using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ChatApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Sprite PlayerChatIcon;
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
    public ChatSelectionUI MessageSelectionUI;
    public ClueSelectionUI ClueSelectionUI;
    public ImageSelectionUI ImageSelectionUI;
    public ChatSelectionUI EnterMessageButtons;

    //Audio
    public AudioSource typingSFX;
    public AudioSource messageSFX;

    // internal
    private List<ChatSelectionUI> m_chatSelectionObjects;
    private ChatScriptableObject m_activeChat;

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
        ChatRunner.NeedsSave += HandleChatNeedsSave;

        // this is a little hard-code-y but idgaf
        m_chatSelectionObjects = new List<ChatSelectionUI>() {
            MessageSelectionUI,
            ClueSelectionUI,
            ImageSelectionUI,
            EnterMessageButtons
        };
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
            CloseOtherChatSelectionUI(null, false);
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
    // Methods : Public (Probably Mostly Buttons)
    // ------------------------------------------------------------------------
    public void OpenChatSelection () {
        CloseChat();

        // set title
        FriendTitleText.text = "Chats";

        // populate list of chat buttons
        // we do this every time we open the app in case it's changed
        foreach(ChatScriptableObject chat in PhoneOS.ActiveChats) {
            GameObject chatButtonObj = Instantiate(
                ChatButtonPrefab,
                ChatButtonsParent
            ) as GameObject;

            OpenChatButton chatButton = chatButtonObj.GetComponent<OpenChatButton>();
            if(chatButton) {
                // set name
                chatButton.NameText.text = chat.DisplayName;

                // set profile pic
                // group chat icon by default
                // TODO: maybe generate an icon that's all of the
                //       friend's icons mushed together?
                Sprite icon = PhoneOS.GameData.GroupChatIcon;
                // if there's only one participant, set it to their icon
                if(chat.Friends.Count == 1) {
                    FriendScriptableObject friend = 
                        PhoneOS.GameData.GetFriend(chat.Friends[0]);  
                    if(friend == null) {
                        Assert.IsNotNull(
                            friend,
                            "Could not find friend[0] for chat " + chat.ID
                        );
                    } else {
                        icon = friend.Icon;
                    }
                }
                chatButton.ProfilePic.sprite = icon;

                // show unread notif (if unfinished)
                if(chat.Finished) {
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
    public void OpenChat (ChatScriptableObject c) {
        if(c == null) {
            Debug.LogError("Trying to open chat with null chat");
            return;
        }
        CloseChatSelection();
        m_activeChat = c;

        //Debug.Log("opening chat: " + c.Friend + "; last visited message: " + m_activeChat.GetLastVisitedMessage().Node);

        // set name
        FriendTitleText.text = c.DisplayName;

        // draw chat bubbles for all of the messages we've read so far
        ChatRunner.RunAllVisitedMessages(c);

        // start chat
        ChatScreen.SetActive(true);
        ChatRunner.StartConversation(m_activeChat);

        ScrollChat();
    }

    // ------------------------------------------------------------------------
    public void OpenClueSelection () {
        ClueSelectionUI.Open(m_activeChat);
        
        if(ImageSelectionUI.IsOpen) {
            ImageSelectionUI.Close();
        }
    }

    // ------------------------------------------------------------------------
    public void OpenSendImage () {
        ImageSelectionUI.Open(m_activeChat);
        
        if(ClueSelectionUI.IsOpen) {
            ClueSelectionUI.Close();
        }
    }

    // ------------------------------------------------------------------------
    public void OpenAttachment (PhotoID photoID) {
        PhotoScriptableObject photo = PhoneOS.GameData.GetPhoto(photoID);
        FullscreenImage.Open(photo.Image);
    }

    // ------------------------------------------------------------------------
    // Methods : ChatRunner event handlers
    // ------------------------------------------------------------------------
    private void HandleChatNeedsSave () {
        Save();
    }

    // ------------------------------------------------------------------------
    private void DrawChatBubble (MessageScriptableObject message, int messageIndex) {
        // determine which prefab to use
        GameObject prefab;
        if(message.Player) {
            prefab = PlayerChatBubblePrefab;
        } else {
            prefab = FriendChatBubblePrefab;
        }

        // determine text and icon
        string text = PreprocessMessage(message.Messages[messageIndex]);

        Sprite sprite = PlayerChatIcon;
        if(!message.Player) {
            FriendScriptableObject friend = PhoneOS.GameData.GetFriend(
                message.Sender
            );
            if(friend == null) {
                Assert.IsNotNull(
                    friend,
                    "Could not find friend for message " + message.Node
                );
                return;
            }
            sprite = friend.Icon;
        }

        // draw bubble 
        ChatBubbleUI chatBubbleUi = CreateChatBubble(
            prefab,
            text,
            sprite,
            message.PostTime
        );

        // create button to show attachment (if has one)
        if(messageIndex == message.Messages.Length - 1 && message.Image != PhotoID.NoPhoto) {
            chatBubbleUi.AddAttachment(
                message.Messages[messageIndex],
                message.Image,
                this
            );
        }
    }

    // ------------------------------------------------------------------------
    private void DrawChatOptionBubble (MessageScriptableObject message, int optionIndex) {
        GameObject option = Instantiate(
            MessageOptionPrefab,
            ChatOptionsParent
        ) as GameObject;

        // setup bubble
        MessageButton messageButton = option.GetComponent<MessageButton>();

        // process dialogue
        string text = PreprocessMessage(message.Messages[optionIndex]);
        
        // set button text & hook up option function
        messageButton.Text.text = text;
        messageButton.Button.onClick.AddListener(
            delegate {ChatRunner.SelectOption(message, optionIndex);}
        );
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
    private void HandleSelectedClueOption (ClueScriptableObject clue) {
        // hide clue selection UI
        MessageSelectionUI.Open();
        CloseOtherChatSelectionUI(MessageSelectionUI, true);
        
        // draw selected option chat bubble
        DrawChatBubble(clue.Message, 0);

        // play sfx
        typingSFX.Play();
    }

    // ------------------------------------------------------------------------
    private void HandleReachedLeafNode () {
        // show clue selection button
        EnterMessageButtons.Open();
        CloseOtherChatSelectionUI(EnterMessageButtons, true);
    }

    // ------------------------------------------------------------------------
    private void HandleFinishedChat (ChatScriptableObject chat) {
        // TODO : not really sure what this means anymore
        // considering all leaf nodes now trigger showing the clue options
        // maybe this is when we run out of clues?
    }

    // ------------------------------------------------------------------------
    // Methods : Private
    // ------------------------------------------------------------------------
    // preprocess messages before displaying (eg, name/pronoun replacement)
    private string PreprocessMessage (string text) {
        return DialogueProcesser.PreprocessDialogue(
            text,
            PhoneOS.Settings.Name,
            PhoneOS.Settings.PronounPersonalSubject,
            PhoneOS.Settings.PronounPersonalObject,
            PhoneOS.Settings.PronounPossessive
        );
    }

    // message-agnostic chat bubble drawing function
    private ChatBubbleUI CreateChatBubble(
        GameObject bubblePrefab,
        string text,
        Sprite icon,
        DateTime timePosted
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
        chatBubbleUi.Setup(text, icon, timePosted);

        // play audio
        messageSFX.Play();

        // mark for needing scroll
        ScrollChat();

        return chatBubbleUi;
    }

    // ------------------------------------------------------------------------
    public void CloseOtherChatSelectionUI (ChatSelectionUI ui, bool drawLine) {
        foreach(ChatSelectionUI obj in m_chatSelectionObjects) {
            if(obj != ui) {
                obj.Close();
            }
        }
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
