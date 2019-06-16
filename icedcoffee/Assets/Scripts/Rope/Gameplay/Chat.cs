using System;   // serializable
using System.Collections.Generic;

public class Message {
    // ------------------------------------------------------------------------
    // Public Variables
    // ------------------------------------------------------------------------
    // the selected option (if any)
    public int OptionSelection;

    // all of the messages to play
    public string[] Messages;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // identifier for this message
    private int m_node;
    public int Node {get{return m_node;}}

    // is this the player speaking or not
    private bool m_player;
    public bool Player {get{return m_player;}}

    // the clue this message gives (if any)
    private ClueID m_clueGiven;
    public ClueID ClueGiven {get{return m_clueGiven;}}

    // converstaion options (leave empty if it's a non-player node or this node has messages)
    private string[] m_options;
    public string[] Options {get{return m_options;}}

    // clue required to selection the option
    private ClueID[] m_clueNeeded;
    public ClueID[] ClueNeeded {get{return m_clueNeeded;}}

    // clue that instigates this message
    private ClueID m_clueTrigger;
    public ClueID ClueTrigger {get{return m_clueTrigger;}}

    // option destinations (by message node)
    // branch = -1 means this is a leaf node
    private int[] m_branch;
    public int[] Branch {get{return m_branch;}}

    // the index in the resources of the attached image
    private PhotoID m_image;
    public PhotoID Image {get{return m_image;}}

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public Message (MessageSerializable serializedMessage) {
        m_node = serializedMessage.node;
        m_player = serializedMessage.player;
        m_clueGiven = serializedMessage.clueGiven;
        Messages = serializedMessage.messages;
        m_options = serializedMessage.options;
        m_clueNeeded = serializedMessage.clueNeeded;
        m_branch = serializedMessage.branch;
        m_image = serializedMessage.image;
        m_clueTrigger = serializedMessage.clueTrigger;

        OptionSelection = -1;
    }

    // ------------------------------------------------------------------------
    public bool MadeSelection () {
        if(!HasOptions()) return true;
        return OptionSelection >= 0;
    }

    // ------------------------------------------------------------------------
    public bool HasOptions () {
        return m_options != null && m_options.Length > 0;
    }
}

public class Chat {
    // ------------------------------------------------------------------------
    // Public Variables
    // ------------------------------------------------------------------------
    // whether or not the convo is finished
    public bool finished;

    // all of the clues we've presented so far
    public List<ClueID> presentedClues;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // the conversation partner
    private Friend m_friend;
    public Friend Friend {get{return m_friend;}}

    // their icon
    private int m_icon;
    public int Icon {get{return m_icon;}}

    // the clue required to open the chat 
    private ClueID m_clueNeeded;
    public ClueID ClueNeeded {get{return m_clueNeeded;}}

    // all of the messages you trade with them
    private Message[] m_messages;
    public bool HasMessages {
        get{return m_messages != null && m_messages.Length > 0;}
    }

    // only the messages you've visited so far
    private List<Message> m_visitedMessages;
    public List<Message> VisitedMessages {get{return m_visitedMessages;}}

    // the index (in 'visitedMessages') of the last node read
    private int m_lastVisitedMessage = 0;
    public int LastVisitedMessage {get{return m_lastVisitedMessage;}}

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public Chat (ChatSerializable serializedChat) {
        m_friend = serializedChat.friend;
        m_clueNeeded = serializedChat.clueNeeded;
        m_icon = serializedChat.icon;

        m_lastVisitedMessage = 0;

        if(serializedChat.messages == null) {
            return;
        }

        m_messages = new Message[serializedChat.messages.Length];
        for (int i = 0; i < m_messages.Length; i++) {
            m_messages[i] = new Message(serializedChat.messages[i]);
        }

        m_visitedMessages = new List<Message>();
        m_visitedMessages.Add(m_messages[0]);

        presentedClues = new List<ClueID>();
    }

    // ------------------------------------------------------------------------
    public void VisitMessage (Message m, bool force) {
        if(m_visitedMessages == null || m == null) {
            return;
        }
        
        // if we're looping back to a multiple-answer question,
        // don't add it again
        if(!force && m_visitedMessages.Contains(m) && (m.Options == null || m.Options.Length > 1)) {
            return;
        }

        if(m_visitedMessages[m_visitedMessages.Count - 1].Node == m.Node) {
            return;
        }

        m_visitedMessages.Add(m);
        m_lastVisitedMessage = m_visitedMessages.Count - 1;
    }

    // ------------------------------------------------------------------------
    public Message GetMessage(int n) {
        foreach(Message m in m_messages) {
            if(m.Node == n) {
                return m;
            }
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public Message GetLastVisitedMessage () {
        return m_visitedMessages[m_lastVisitedMessage];
    }

    // ------------------------------------------------------------------------
    public Message GetMessageWithClueTrigger (ClueID trigger) {
        foreach(Message m in m_messages) {
            if(m.ClueTrigger == trigger) {
                return m;
            }
        }
        return null;
    }
}
