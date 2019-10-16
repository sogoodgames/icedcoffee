using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct ChatProgressionData {
    public Friend Friend;
    // only the messages you've visited so far
    public List<MessageProgressionData> VisitedMessages;
    // the index (in 'visitedMessages') of the last node read
    public int LastVisitedMessage;
    public List<ClueID> PresentedClues;
    public bool Finished;

    public ChatProgressionData (
        Friend friend,
        List<MessageProgressionData> visitedMessages,
        int lastMessage,
        List<ClueID> presentedClues,
        bool finishedChat
    ) {
        Friend = friend;
        VisitedMessages = visitedMessages;
        LastVisitedMessage = lastMessage;
        PresentedClues = presentedClues;
        Finished = finishedChat;
    }
}

[CreateAssetMenu(fileName = "ChatData", menuName = "IcedCoffee/ChatScriptableObject", order = 1)]
public class ChatScriptableObject : ScriptableObject
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend Friend; // the person you're talking to
    public Sprite Icon; // icon file
    public ClueID ClueNeeded; // the clue needed to unlock the chat
    public MessageScriptableObject[] Messages; // all of the messages
    
    private ChatProgressionData m_progressionData;
    
    // public accessors for progression data
    public int LastVisitedMessage {get{return m_progressionData.LastVisitedMessage;}}
    public bool Finished {get{return m_progressionData.Finished;}}
    public List<ClueID> PresentedClues {get{return m_progressionData.PresentedClues;}}

    // caches all visited messages from progression data
    private List<MessageScriptableObject> m_visitedMessages;
    public List<MessageScriptableObject> VisitedMessages {get{return m_visitedMessages;}}

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------ 
    public void ClearProgression () {
        m_progressionData = new ChatProgressionData(
            Friend, 
            new List<MessageProgressionData>(),
            0,
            new List<ClueID>(),
            false
        );

        m_visitedMessages = new List<MessageScriptableObject>();
        AddMessageToProgression(Messages[0]);
        //Debug.Log("called clear progression on convo: " + Friend);
    }

    // ------------------------------------------------------------------------
    public void LoadProgression(ChatProgressionData progressionData) {
        m_progressionData = progressionData;
    }

    // ------------------------------------------------------------------------ 
    public void MarkComplete() {
        m_progressionData.Finished = true;
    }

    // ------------------------------------------------------------------------
    public void VisitMessage (MessageScriptableObject m, bool force) {
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
        //Debug.Log("visited messages lenght: " + m_visitedMessages.Count);
        //Debug.Log("index: " + m_progressionData.LastVisitedMessage);
        return m_visitedMessages[m_progressionData.LastVisitedMessage];
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
            new MessageProgressionData(m.Node, 0, false)
        );
        m_visitedMessages.Add(m);
        // THEN set last visited message index
        m_progressionData.LastVisitedMessage = m_visitedMessages.Count - 1;
        Debug.Log("added message to progression: " + m.Node);
    }
}