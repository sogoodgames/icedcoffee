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
    public Transform ChatButtonsParent;
    public GameObject ChatButtonPrefab;

    // chat messages screen
    public Text FriendTitleText;
    public Transform ChatBubblesParent;
    public Transform ChatOptionsParent;
    public GameObject PlayerChatBubblePrefab;
    public GameObject FriendChatBubblePrefab;
    public GameObject MessageOptionPrefab;
    public ScrollRect ChatBubblesScrollRect;
    public ChatAttachment ChatAttachment;

    //Audio
    public AudioSource typingSFX;
    public AudioSource messageSFX;

    // internal
    private bool m_needsScroll;
    private Chat m_activeChat;
    private int scrollWait = 0;

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
    // why is 3 the magic number of frames to get an accurate scroll position?
    // i have no idea.
    // why am i not using a more elegant solution using ienumerators?
    // because i'm tired.
    protected void Update() {
        if(m_needsScroll) {
            scrollWait++;
            if(scrollWait >= 3) {
                ChatBubblesScrollRect.normalizedPosition = new Vector2(0, 0);
                m_needsScroll = false;
                scrollWait = 0;
            }
        }
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
            ChatAttachment.Close();
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
                chatButton.ProfilePic.sprite = PhoneOS.DataLoader.UserIconAssets[chat.Icon];

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
            chatBubbleUi.Icon.sprite = PhoneOS.DataLoader.UserIconAssets[m_activeChat.Icon];
        }

        if(messageIndex == message.Messages.Length - 1 && message.Image >= 0) {
            chatBubbleUi.Text.text = message.Messages[messageIndex] + " [click to open attachment]";
            chatBubbleUi.Button.onClick.AddListener(
                delegate {OpenAttachment(message);}
            );
            chatBubbleUi.Button.interactable = true;
        }

        // play audio
        messageSFX.Play();

        // mark for needing scroll
        m_needsScroll = true;
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
        if(chat.Friend == Friend.Jin) {
            ChatAttachment.Open(PhoneOS.DataLoader.JinEndingPhoto/*, 1080, 810*/);
        } else if(chat.Friend == Friend.Emma && chat.GetLastVisitedMessage().Node == 16) {
            ChatAttachment.Open(PhoneOS.DataLoader.EmmaEndingPhoto/*, 1078, 1437*/);
        }
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
        ChatAttachment.Open(PhoneOS.DataLoader.PhotoAssets[photo.Image]);
    }

    // ------------------------------------------------------------------------
    // Methods : Closing UI
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
}
