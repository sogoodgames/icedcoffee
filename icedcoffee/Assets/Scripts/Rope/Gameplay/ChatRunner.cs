using UnityEngine;
using System.Collections;

public class ChatRunner : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    // hook into these with your UI class in order to draw chat bubbles
    public delegate void MessageDelegate(Message message, int messageIndex);
    public event MessageDelegate VisitedMessage;

    public delegate void OptionDelegate(Message message, int optionIndex);
    public event OptionDelegate VisitedOption;
    
    // information for gameplay
    public delegate void FinishedChatDelegate(Chat chat);
    public event FinishedChatDelegate FinishedChat;

    public delegate void FoundClueDelegate(ClueID clueID);
    public event FoundClueDelegate FoundClue;

    public delegate void SelectedOptionDelegate();
    public event SelectedOptionDelegate SelectedOption;

    public float MaxTimeBetweenMessages = 2f;

    private Chat m_activeChat;

    private IEnumerator m_RunMessageCoroutine;
    private IEnumerator m_RunBubblesCoroutine;

    // ------------------------------------------------------------------------
    // Methods : Public
    // ------------------------------------------------------------------------
    public void StartConversation (Chat chat) {
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
    public void RunAllVisitedMessages (Chat chat) {
        foreach(Message message in chat.VisitedMessages) {
            // record any clues found
            // in case the player missed them last time
            if(message.ClueGiven != ClueID.NoClue) {
                FoundClue(message.ClueGiven);
            }

            // run message
            if(message.Player) {
                if(message.HasOptions()) {
                    if(message.MadeSelection()) {
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

        if(m_activeChat.finished) {
            return;
        }

        Message lastMessage = m_activeChat.GetLastVisitedMessage();
        int nextNode = -1;

        // find next message in convo
        if(lastMessage.HasOptions()) {
            if(lastMessage.MadeSelection()) {
                // if we made a selection, move to the next message
                nextNode = lastMessage.Branch[lastMessage.OptionSelection];
            } else {
                // if we have an unchosen option, don't do anything
                return;
            }
        } else {
            nextNode = lastMessage.Branch[0];
        }

        // draw the next message
        Message nextMessage = m_activeChat.GetMessage(nextNode);
        if(nextMessage == null){
            MarkConversationComplete();
            return;
        }
        m_RunMessageCoroutine = RunMessage(nextMessage);
        StartCoroutine(m_RunMessageCoroutine);
    }

    // ------------------------------------------------------------------------
    private IEnumerator RunMessage (Message message) {
        if(message == null) {
            Debug.LogError("Message null.");
            yield break;
        }

        // record that we visited this message (don't force)
        m_activeChat.VisitMessage(message, false);

        // draw either player or friend messages
        if(message.Player) {
            // if this has options, draw them; otherwise, draw messages
            if(message.HasOptions()) {
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

        // if we're not waiting on an option selection, draw the next message
        if(!message.HasOptions()) {
            MoveConversation();
        }
    }

    // ------------------------------------------------------------------------
    // actually does the waiting to create delay between messages
    private IEnumerator RunChatBubbles (Message message) {
        if(message == null) {
            Debug.LogError("Message null.");
            yield break;
        }

        // visit all of the messages in this node
        for (int i = 0; i < message.Messages.Length; i++) {
            float t = 2;
            if((message.Node == 0 && i == 0) || message.HasOptions()) {
                t = 0;
            }
            yield return new WaitForSeconds(t);
            //Debug.Log("drawing line: " + i);

            VisitedMessage(message, i);
        }
    }

    // ------------------------------------------------------------------------
    private void RunChatOptions (Message message) {
        if(message == null) {
            Debug.LogError("Message null.");
            return;
        }
        if(!message.Player) {
            Debug.LogError("Hecked up script config. NPC has chat options.");
            return;
        }
        if(!message.HasOptions()) {
            Debug.LogError("Attempting to draw options for message with no options.");
            return;
        }

        // if we've answered this question multiple times, mark this convo done
        if(m_activeChat.VisitedMessages.FindAll(m => m.Node == message.Node).Count > 1) {
            MarkConversationComplete();
            return;
        }

        for(int i = 0; i < message.Options.Length; i++) {
            // if we've already been to this conversation option,
            // skip drawing whatever option we selected last time
            if(message.MadeSelection() && i == message.OptionSelection) {
                continue;
            }
            VisitedOption(message, i);
        }
    }

    // ------------------------------------------------------------------------
    public void SelectOption (Message message, int option) {
        if(message == null) {
            Debug.LogError("Message null.");
            return;
        }

        //Debug.Log("selected option " + option + " for message " + message.Node);

        // record in message that this option has been chosen
        message.OptionSelection = option;
        message.Messages = new string[1];
        message.Messages[0] = message.Options[option];

        // run chosen message
        m_RunBubblesCoroutine = RunChatBubbles(message);
        StartCoroutine(m_RunBubblesCoroutine);

        // force record that we visited this message
        m_activeChat.VisitMessage(message, true);

        // fire event
        SelectedOption();

        // run next chat
        MoveConversation();
    }

    // ------------------------------------------------------------------------
    private void MarkConversationComplete () {
        //Debug.Log("Reached end of convo at node " + m_activeChat.GetLastVisitedMessage().Node);
        m_activeChat.finished = true;
        FinishedChat(m_activeChat);
    }
}