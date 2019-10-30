using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[Serializable]
public struct ChatProgressionData {
    public int ID;
    // only the messages you've visited so far
    public List<MessageProgressionData> VisitedMessages;
    public List<ClueID> PresentedClues;
    public bool Finished;

    // ------------------------------------------------------------------------
    public ChatProgressionData (
        int id,
        List<MessageProgressionData> visitedMessages,
        List<ClueID> presentedClues,
        bool finishedChat
    ) {
        ID = id;
        VisitedMessages = visitedMessages;
        PresentedClues = presentedClues;
        Finished = finishedChat;
    }

    // ------------------------------------------------------------------------
    public void LogData () {
        Debug.Log("> chat: " + ID);
        Debug.Log(">> messages: ");
        foreach(MessageProgressionData msg in VisitedMessages) {
            Debug.Log(">>> " + msg.Node + "; made selection: " + msg.MadeSelection);
        }
    }
}

[CreateAssetMenu(fileName = "ChatData", menuName = "IcedCoffee/ChatScriptableObject", order = 1)]
public class ChatScriptableObject : ScriptableObject
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public ClueID ClueNeeded; // the clue needed to unlock the chat
    public MessageScriptableObject[] Messages; // all of the messages
    
    private ChatProgressionData m_progressionData;
    
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // auto-generated ID
    [SerializeField]
    private int m_id;
    public int ID {get{return m_id;}}

    // public accessors for progression data
    public ChatProgressionData ProgressionData {get{return m_progressionData;}}
    public bool Finished {get{return m_progressionData.Finished;}}
    public List<ClueID> PresentedClues {
        get{return m_progressionData.PresentedClues;}
    }

    // caches all visited messages
    // (since progression data stores progression objects, 
    //  not message objects themselves)
    private List<MessageScriptableObject> m_visitedMessages;
    public List<MessageScriptableObject> VisitedMessages { 
        get{return m_visitedMessages;}
    }

    // all of the friends in the chat
    public List<Friend> Friends {
        get {
            List<Friend> friends = new List<Friend>();
            if(Messages != null) {
                foreach(MessageScriptableObject m in Messages) {
                    if(m != null
                       && !friends.Contains(m.Sender)
                       && m.Sender != Friend.You
                    ){
                        friends.Add(m.Sender);
                    }
                }
            }
            return friends;
        }
    }

    // display name for UI, debug
    public string DisplayName {
        get {
            if(Messages == null) {
                return "New Chat";
            }

            StringBuilder sb = new StringBuilder("");
            List<Friend> friends = Friends;
            for (int i = 0; i < friends.Count; i++) {
                sb.Append(friends[i].ToString());
                if(i != friends.Count - 1) {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void ClearProgression () {
        m_progressionData = new ChatProgressionData(
            m_id, 
            new List<MessageProgressionData>(),
            new List<ClueID>(),
            false
        );

        foreach(MessageScriptableObject messageObj in Messages) {
            messageObj.ClearProgression();
        }

        m_visitedMessages = new List<MessageScriptableObject>();
        AddMessageToProgression(Messages[0]);
        //Debug.Log("called clear progression on convo: " + Friend);
    }

    // ------------------------------------------------------------------------
    public void LoadProgression (
        ChatProgressionData progressionData,
        List<ClueScriptableObject> gameClueData
    ) {
        m_progressionData = progressionData;

        // create visited messages cache
        m_visitedMessages = new List<MessageScriptableObject>();
        foreach(MessageProgressionData msgProgression in m_progressionData.VisitedMessages) {
            // find corresponding message object based on ID
            MessageScriptableObject msgObject;
            if(msgProgression.IsClueMessage) {
                // if it's a clue message, search clues
                msgObject = gameClueData.First (
                    c => c.Message != null && c.Message.Node == msgProgression.Node
                ).Message;
            }
            else {
                // otherwise, search this chat's messages
                msgObject = Messages.First (
                    m => m.Node == msgProgression.Node
                );
            }

            if(msgObject == null) {
                Debug.LogError(
                    "Corresponding message object not found for progression: "
                    + msgProgression.Node
                );
                continue;
            }

            // give message object progression data
            msgObject.LoadProgression(msgProgression);

            // add message object to list of visited messages
            m_visitedMessages.Add(msgObject);
        }
    }

    // ------------------------------------------------------------------------ 
    public void MarkComplete() {
        m_progressionData.Finished = true;
    }

    // ------------------------------------------------------------------------
    public void RecordMessageInProgression (MessageScriptableObject m, bool force) {
        if(m_visitedMessages == null || m == null) {
            return;
        }
        
        // if we're looping back to a multiple-answer question,
        // don't add it again
        if(!force && m_visitedMessages.Contains(m) && (m.Options == null || m.Options.Length > 1)) {
            return;
        }

        // if we just logged this message, don't log again
        // TODO: why is this here and what is it hiding
        if(m_visitedMessages[m_visitedMessages.Count - 1].Node == m.Node) {
            return;
        }

        // if this message has options and we haven't selected one yet,
        // don't add it yet
        if(!force && m.HasOptions && !m.MadeSelection) {
            return;
        }

        AddMessageToProgression(m);
    }

    // ------------------------------------------------------------------------
    public MessageScriptableObject GetMessage(int n) {
        foreach(MessageScriptableObject m in Messages) {
            if(m.Node == n) {
                return m;
            }
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public MessageScriptableObject GetLastVisitedMessage () {
        return m_visitedMessages[m_visitedMessages.Count - 1];
    }

    // ------------------------------------------------------------------------
    public MessageScriptableObject GetMessageWithClueTrigger (ClueID trigger) {
        foreach(MessageScriptableObject m in Messages) {
            if(m.ClueTrigger == trigger) {
                return m;
            }
        }
        return null;
    }

    // ------------------------------------------------------------------------
    private void AddMessageToProgression (MessageScriptableObject m) {
        // first add messages to progression data and cached data
        m_progressionData.VisitedMessages.Add(
            new MessageProgressionData(m.Node, 0, m.MadeSelection, m.IsClueMessage)
        );
        m_visitedMessages.Add(m);
        //Debug.Log("added message to progression: " + m.Node);
    }
}