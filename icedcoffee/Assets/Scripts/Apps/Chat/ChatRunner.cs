using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

public class ChatRunner : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    // hook into these with your UI class in order to draw chat bubbles
    public delegate void MessageDelegate(MessageScriptableObject message, int messageIndex);
    public event MessageDelegate VisitedMessage;

    public delegate void OptionDelegate(MessageScriptableObject message, int optionIndex);
    public event OptionDelegate VisitedOption;

    public delegate void LeafNodeDelegate();
    public event LeafNodeDelegate ReachedLeafNode;

    public delegate void ClueOptionDelegate(ClueScriptableObject clue);
    public event ClueOptionDelegate VisitedClueOption;
    
    // events for gameplay
    public delegate void FinishedChatDelegate(ChatScriptableObject chat);
    public event FinishedChatDelegate FinishedChat;

    public delegate void FoundClueDelegate(ClueID clueID);
    public event FoundClueDelegate FoundClue;

    public delegate void SelectedOptionDelegate();
    public event SelectedOptionDelegate SelectedOption;

    public delegate void NeedsSaveDelegate();
    public event NeedsSaveDelegate NeedsSave;

    // settings
    public float MaxTimeBetweenMessages = 2f;

    private ChatScriptableObject m_activeChat;

    private IEnumerator m_RunMessageCoroutine;
    private IEnumerator m_RunBubblesCoroutine;

    // ------------------------------------------------------------------------
    // Methods : Public
    // ------------------------------------------------------------------------
    public void StartConversation (ChatScriptableObject chat) {
        m_activeChat = chat;
        MoveConversation();
    }

    // ------------------------------------------------------------------------
    // doesn't complete convo, just stops coroutines
    public void StopActiveConversation () {
        if(m_RunMessageCoroutine != null) {
            StopCoroutine(m_RunMessageCoroutine);
        }
        if(m_RunBubblesCoroutine != null) {
            StopCoroutine(m_RunBubblesCoroutine);
        }
    }

    // ------------------------------------------------------------------------
    public void RunAllVisitedMessages (ChatScriptableObject chat) {
        foreach(MessageScriptableObject message in chat.VisitedMessages) {
            // record any clues found
            // in case the player missed them last time
            if(message.ClueGiven != ClueID.NoClue) {
                FoundClue(message.ClueGiven);
            }

            // run message
            if(message.Player) {
                if(message.HasOptions) {
                    if(message.MadeSelection) {
                        VisitedMessage(message, 0);
                    } else {
                        RunChatOptions(message);
                    }
                } else {
                    for (int i = 0; i < message.Messages.Length; i++) {
                        VisitedMessage(message, i);
                    }
                }
            } else {
                for (int i = 0; i < message.Messages.Length; i++) {
                    VisitedMessage(message, i);
                }
            }
        }
    }

    // ------------------------------------------------------------------------
    // Methods : Private
    // ------------------------------------------------------------------------
    private void MoveConversation () {
        if(m_activeChat == null) {
            Debug.Log("Trying to move in conversation that hasn't been set.");
            return;
        }

        if(m_activeChat.Finished) {
            return;
        }

        MessageScriptableObject lastMessage = m_activeChat.GetLastVisitedMessage();

        // find next message in convo (if this isn't a leaf)
        if(lastMessage.HasBranch) {
            int nextNode = -1;
            // find the next message's node
            if(lastMessage.HasOptions) {
                if(lastMessage.MadeSelection) {
                    // if we made a selection, move to the next message
                    nextNode = lastMessage.Branch[lastMessage.OptionSelection];
                } else {
                    // if we have an unchosen option, don't do anything
                    return;
                }
            } else {
                nextNode = lastMessage.Branch[0];
            }
            // run it
            MessageScriptableObject nextMessage = m_activeChat.GetMessage(nextNode);
            if(nextMessage != null) {
                m_RunMessageCoroutine = RunMessage(nextMessage);
                StartCoroutine(m_RunMessageCoroutine);
            } else {
                Assert.IsNotNull(
                    nextMessage,
                    "Could not find message with node " + nextNode +
                    " in chat" + m_activeChat.Friend
                );
            }
        } else {
            // if this is a leaf node, send leaf node event
            // don't run more messages
            ReachedLeafNode();
        }
    }

    // ------------------------------------------------------------------------
    private IEnumerator RunMessage (MessageScriptableObject message) {
        if(message == null) {
            Debug.LogError("Message null.");
            yield break;
        }

        // record that we visited this message (don't force)
        m_activeChat.RecordMessageInProgression(message, false);

        // draw either player or friend messages
        if(message.Player) {
            // if this has options, draw them; otherwise, draw messages
            if(message.HasOptions) {
                RunChatOptions(message);
            } else {
                m_RunBubblesCoroutine = RunChatBubbles(message);
                yield return StartCoroutine(m_RunBubblesCoroutine);
            }
        } else {
            m_RunBubblesCoroutine = RunChatBubbles(message);
            yield return StartCoroutine(m_RunBubblesCoroutine);
        }

        // record any clues found
        if(message.ClueGiven != ClueID.NoClue) {
            FoundClue(message.ClueGiven);
        }

        NeedsSave();

        // if we're not waiting on an option selection, draw the next message
        if(!message.HasOptions) {
            MoveConversation();
        }
    }

    // ------------------------------------------------------------------------
    // actually does the waiting to create delay between messages
    private IEnumerator RunChatBubbles (MessageScriptableObject message) {
        if(message == null) {
            Debug.LogError("Message null.");
            yield break;
        }

        // visit all of the messages in this node
        for (int i = 0; i < message.Messages.Length; i++) {
            float t = MaxTimeBetweenMessages;
            if((message.Node == 0 && i == 0) || message.HasOptions) {
                t = 0;
            }
            yield return new WaitForSeconds(t);
            //Debug.Log("drawing line: " + i);

            VisitedMessage(message, i);
        }
    }

    // ------------------------------------------------------------------------
    private void RunChatOptions (MessageScriptableObject message) {
        if(message == null) {
            Debug.LogError("Message null.");
            return;
        }
        if(!message.Player) {
            Debug.LogError("Hecked up script config. NPC has chat options.");
            return;
        }
        if(!message.HasOptions) {
            Debug.LogError("Attempting to draw options for message with no options.");
            return;
        }

        // if we've answered this question multiple times, mark this convo done
        /*if(m_activeChat.VisitedMessages.FindAll(m => m.Node == message.Node).Count > 1) {
            MarkConversationComplete();
            return;
        }*/

        for(int i = 0; i < message.Options.Length; i++) {
            // if we've already been to this conversation option,
            // skip drawing whatever option we selected last time
            if(message.MadeSelection && i == message.OptionSelection) {
                continue;
            }
            VisitedOption(message, i);
        }
    }

    // ------------------------------------------------------------------------
    // selected a generic (non-clue-related) chat option
    public void SelectOption (MessageScriptableObject message, int option) {
        if(message == null) {
            Debug.LogError("Message null.");
            return;
        }

        //Debug.Log("selected option " + option + " for message " + message.Node);

        // record in message that this option has been chosen
        message.Messages = new string[1];
        message.Messages[0] = message.Options[option];

        // run chosen message
        m_RunBubblesCoroutine = RunChatBubbles(message);
        StartCoroutine(m_RunBubblesCoroutine);

        // FIRST update the message's data that we visited it
        message.SelectOption(option);
        // THEN force record that we visited this message
        m_activeChat.RecordMessageInProgression(message, true);

        // fire events
        SelectedOption();
        NeedsSave();

        // run next chat
        MoveConversation();
    }

    // ------------------------------------------------------------------------
    // presenting a clue to the conversation
    public void SelectClueOption (ClueScriptableObject clue) {
        //Debug.Log("presenting clue: " + clue.ClueID);
        MessageScriptableObject message = m_activeChat.GetMessageWithClueTrigger(clue.ClueID);
        
        if(message == null) {
            Debug.LogError("Can't find message with clueTrigger " + clue.ClueID);
        } else {
            // log that we've presented this clue
            m_activeChat.PresentedClues.Add(clue.ClueID);

            // fire event for UI
            VisitedClueOption(clue);

            // log the message 
            m_activeChat.RecordMessageInProgression(clue.Message, true);

            // fire event for saving
            NeedsSave();

            // run message triggered by this option
            m_RunMessageCoroutine = RunMessage(message);
            StartCoroutine(m_RunMessageCoroutine);
        }
    }

    // ------------------------------------------------------------------------
    private void MarkConversationComplete () {
        //Debug.Log("Reached end of convo at node " + m_activeChat.GetLastVisitedMessage().Node);
        m_activeChat.MarkComplete();
        FinishedChat(m_activeChat);
        NeedsSave();
    }
}