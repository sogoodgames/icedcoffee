using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
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
    // Methods : ChatRunner event handlers
    // ------------------------------------------------------------------------
    private void DrawChatBubble (Message message, int messageIndex) {
        // decided which prefab to use
        GameObject prefab;
        if(message.Player) {
            prefab = PlayerChatBubblePrefab;
        } else {
            prefab = FriendChatBubblePrefab;
        }

        // create bubble object
        GameObject bubble = Instantiate(
            prefab,
            ChatBubblesParent
        ) as GameObject;

        // find ChatBubbleUI
        ChatBubbleUI chatBubbleUi = bubble.GetComponent<ChatBubbleUI>();
        if(!chatBubbleUi) {
            return;
        }

        // fill text, icon, and image
        chatBubbleUi.Text.text = message.Messages[messageIndex];

        if(!message.Player) {
            chatBubbleUi.Icon.sprite = PhoneOS.GetIcon(m_activeChat.Icon);
        }

        if(messageIndex == message.Messages.Length - 1 && message.Image != PhotoID.NoPhoto) {
            chatBubbleUi.Text.text = message.Messages[messageIndex] + " [click to open attachment]";
            chatBubbleUi.Button.onClick.AddListener(
                delegate {OpenAttachment(message);}
            );
            chatBubbleUi.Button.interactable = true;
        }

        // play audio
        messageSFX.Play();

        // mark for needing scroll
        ScrollChat();
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
            SetButtonListener(messageButton.Button, message, optionIndex);
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
    private void HandleFinishedChat (Chat chat) {
        // TODO
    }

    // ------------------------------------------------------------------------
    // Methods : Buttons
    // ------------------------------------------------------------------------
    private void SetButtonListener(Button b, Message m, int i) {
        b.onClick.AddListener(
            delegate {ChatRunner.SelectOption(m, i);}
        );
    }

    // ------------------------------------------------------------------------
    public void OpenAttachment (Message message) {
        Photo photo = PhoneOS.GetPhoto(message.Image);
        FullscreenImage.Open(PhoneOS.GetPhotoSprite(photo.Image));
    }

    // ------------------------------------------------------------------------
    // Methods : Other Private
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

    private void ScrollChat () {
        LayoutRebuilder.ForceRebuildLayoutImmediate(ChatBubblesParent);
        ChatBubblesScrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
