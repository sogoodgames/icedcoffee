using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public int PlayerChatIconIndex = 0;
    public ChatRunner ChatRunner;
    public GameObject ChatSelectionScreen;
    public GameObject ChatScreen;

    // chat selection screen
    public RectTransform ChatButtonsParent;
    public GameObject ChatButtonPrefab;

    // chat messages screen
    public Text FriendTitleText;
    public RectTransform ChatBubblesParent;
    public RectTransform ChatOptionsParent;
    public GameObject PlayerChatBubblePrefab;
    public GameObject FriendChatBubblePrefab;
    public GameObject MessageOptionPrefab;
    public ScrollRect ChatBubblesScrollRect;
    public FullscreenImage FullscreenImage;

    // clue selection stuff
    public ClueSelectionUI ClueSelectionUI;
    public Button OpenClueSelectionButton;

    //Audio
    public AudioSource typingSFX;
    public AudioSource messageSFX;

    // internal
    private Chat m_activeChat;

    // ------------------------------------------------------------------------
    // Methods : MonoBehaviour
    // ------------------------------------------------------------------------
    protected override void Awake () {
        base.Awake();
        ChatRunner.VisitedMessage += DrawChatBubble;
        ChatRunner.VisitedOption += DrawChatOptionBubble;
        ChatRunner.FinishedChat += HandleFinishedChat;
        ChatRunner.SelectedOption += HandleSelectedOption;
        ChatRunner.ReachedLeafNode += HandleReachedLeafNode;
        ChatRunner.VisitedClueOption += HandleSelectedClueOption;
    }

    // ------------------------------------------------------------------------
    // Methods : App
    // ------------------------------------------------------------------------
    public override void Open() {
        base.Open();
        PhoneOS.ReturnButton.SetActive(true);
        // always reset to chat selection bc i'm lazy lol
        OpenChatSelection();
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        bool waitingForClose = m_waitingForClose;
        base.HandleSlideAnimationFinished();

        if(waitingForClose) {
            m_activeChat = null;
            PhoneOS.ReturnButton.SetActive(false);
            CloseChatSelection();
            CloseChat();
            FullscreenImage.Close();
            ClueSelectionUI.Close();
        }
    }

    // ------------------------------------------------------------------------
    public override void Return() {
        if(ChatScreen.activeInHierarchy) {
            CloseChat();
            OpenChatSelection();
        } else if(m_activeChat != null) {
            CloseChatSelection();
            OpenChat(m_activeChat);
        } else {
            Close();
        }
    }

    // ------------------------------------------------------------------------
    // Methods : Public
    // ------------------------------------------------------------------------
    public void OpenChatSelection () {
        CloseChat();

        // populate list of chat buttons
        // we do this every time we open the app in case it's changed
        foreach(Chat chat in PhoneOS.ActiveChats) {
            GameObject chatButtonObj = Instantiate(
                ChatButtonPrefab,
                ChatButtonsParent
            ) as GameObject;

            OpenChatButton chatButton = chatButtonObj.GetComponent<OpenChatButton>();
            if(chatButton) {
                // set name
                chatButton.NameText.text = chat.Friend.ToString();

                // set profile pic
                chatButton.ProfilePic.sprite = PhoneOS.GetIcon(chat.Icon);

                // show unread notif (if unfinished)
                if(chat.finished) {
                    chatButton.UnreadNotif.SetActive(false);
                } else {
                    chatButton.UnreadNotif.SetActive(true);
                }

                // bind button to open this chat
                chatButton.OpenButton.onClick.AddListener(
                    delegate {OpenChat(chat);}
                );

            } else {
                Debug.LogError("Chat Button Prefab does not contain an OpenChatButton.");
            }
        }

        ChatSelectionScreen.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public void OpenChat (Chat c) {
        if(c == null) {
            Debug.LogError("Trying to open chat with null chat");
            return;
        }
        CloseChatSelection();
        m_activeChat = c;

        //Debug.Log("opening chat: " + c.Friend + "; last visited message: " + m_activeChat.GetLastVisitedMessage().Node);

        // set name
        FriendTitleText.text = c.Friend.ToString();

        // draw chat bubbles for all of the messages we've read so far
        ChatRunner.RunAllVisitedMessages(c);

        // start chat
        ChatScreen.SetActive(true);
        ChatRunner.StartConversation(m_activeChat);
    }

    // ------------------------------------------------------------------------
    public void OpenClueSelection () {
        ClueSelectionUI.Open(m_activeChat);
    }

    // ------------------------------------------------------------------------
    public void OpenAttachment (Message message) {
        Photo photo = PhoneOS.GetPhoto(message.Image);
        FullscreenImage.Open(PhoneOS.GetPhotoSprite(photo.Image));
    }

    // ------------------------------------------------------------------------
    // Methods : ChatRunner event handlers
    // ------------------------------------------------------------------------
    private void DrawChatBubble (Message message, int messageIndex) {
        // determine which prefab to use
        GameObject prefab;
        if(message.Player) {
            prefab = PlayerChatBubblePrefab;
        } else {
            prefab = FriendChatBubblePrefab;
        }

        // determine text and icon
        string text = message.Messages[messageIndex];

        Sprite sprite = PhoneOS.GetIcon(PlayerChatIconIndex);
        if(!message.Player) {
            sprite = PhoneOS.GetIcon(m_activeChat.Icon);
        }

        // draw bubble 
        ChatBubbleUI chatBubbleUi = CreateChatBubble(prefab, text, sprite);

        // create button to show attachment (if has one)
        if(messageIndex == message.Messages.Length - 1 && message.Image != PhotoID.NoPhoto) {
            chatBubbleUi.Text.text = message.Messages[messageIndex] + " [click to open attachment]";
            chatBubbleUi.Button.onClick.AddListener(
                delegate {OpenAttachment(message);}
            );
            chatBubbleUi.Button.interactable = true;
        }
    }

    // ------------------------------------------------------------------------
    private void DrawChatOptionBubble (Message message, int optionIndex) {
        GameObject option = Instantiate(
            MessageOptionPrefab,
            ChatOptionsParent
        ) as GameObject;

        // setup bubble
        MessageButton messageButton = option.GetComponent<MessageButton>();
        
        // check if clue needs met
        if(PhoneOS.ClueRequirementMet(message.ClueNeeded[optionIndex])) {
            // set button text & hook up option function
            messageButton.Text.text = message.Options[optionIndex];
            messageButton.Button.onClick.AddListener(
                delegate {ChatRunner.SelectOption(message, optionIndex);}
            );
            //Debug.Log("created option [" + message.options[i] + "] with index " + i + " for message " + message.node);
        } else {
            // mark it as unavilable
            messageButton.Text.text = "[clue needed]";
        }
    }

    // ------------------------------------------------------------------------
    private void HandleSelectedOption () {
        // destroy option bubbles
        foreach(Transform child in ChatOptionsParent.transform) {
            Destroy(child.gameObject);
        }

        // play sfx
        typingSFX.Play();
    }

    // ------------------------------------------------------------------------
    private void HandleSelectedClueOption (ClueID id) {
        // hide clue selection UI
        ClueSelectionUI.Close();
        OpenClueSelectionButton.gameObject.SetActive(false);

        // draw selected option chat bubble
        Clue clue = PhoneOS.GetClue(id);
        CreateChatBubble(
            PlayerChatBubblePrefab,
            clue.MessageText,
            PhoneOS.GetIcon(PlayerChatIconIndex)
        );

        // play sfx
        typingSFX.Play();
    }

    // ------------------------------------------------------------------------
    private void HandleReachedLeafNode () {
        // show clue selection button
        OpenClueSelectionButton.gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    private void HandleFinishedChat (Chat chat) {
        // TODO : not really sure what this means anymore
        // considering all leaf nodes now trigger showing the clue options
        // maybe this is when we run out of clues?
    }

    // ------------------------------------------------------------------------
    // Methods : Private
    // ------------------------------------------------------------------------
    // message-agnostic chat bubble drawing function
    private ChatBubbleUI CreateChatBubble(
        GameObject bubblePrefab,
        string text,
        Sprite icon
    ) {
        // create bubble object
        GameObject bubble = Instantiate(
            bubblePrefab,
            ChatBubblesParent
        ) as GameObject;

        // find ChatBubbleUI
        ChatBubbleUI chatBubbleUi = bubble.GetComponent<ChatBubbleUI>();
        if(chatBubbleUi == null) {
            Debug.LogError("no chat bubble ui component found on chat bubble prefab");
            return null;
        }

        // fill text and sprite
        chatBubbleUi.Text.text = text;
        chatBubbleUi.Icon.sprite = icon;

        // play audio
        messageSFX.Play();

        // mark for needing scroll
        ScrollChat();

        return chatBubbleUi;
    }

    // ------------------------------------------------------------------------
    private void CloseChat () {
        ChatRunner.StopActiveConversation();

        foreach(Transform child in ChatBubblesParent.transform) {
            Destroy(child.gameObject);
        }
        foreach(Transform child in ChatOptionsParent.transform) {
            Destroy(child.gameObject);
        }
        ChatScreen.SetActive(false);
    }

    // ------------------------------------------------------------------------
    private void CloseChatSelection () {
        foreach(Transform child in ChatButtonsParent.transform) {
            Destroy(child.gameObject);
        }
        ChatSelectionScreen.SetActive(false);
    }

    // ------------------------------------------------------------------------
    private void ScrollChat () {
        LayoutRebuilder.ForceRebuildLayoutImmediate(ChatBubblesParent);
        ChatBubblesScrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
