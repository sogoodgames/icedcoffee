using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChatData", menuName = "IcedCoffee/ChatScriptableObject", order = 1)]
public class ChatScriptableObject : ScriptableObject
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend Friend; // the person you're talking to
    public int Icon; // icon file
    public ClueID ClueNeeded; // the clue needed to unlock the chat
    public MessageScriptableObject[] Messages; // all of the messages

    
    [HideInInspector]
    public bool Finished; // whether or not the convo is finished
    [HideInInspector]
    public List<ClueID> PresentedClues; // all of the clues we've presented so far

    // only the messages you've visited so far
    private List<MessageScriptableObject> m_visitedMessages;
    public List<MessageScriptableObject> VisitedMessages {get{return m_visitedMessages;}}

    // the index (in 'visitedMessages') of the last node read
    private int m_lastVisitedMessage = 0;
    public int LastVisitedMessage {get{return m_lastVisitedMessage;}}

    // ------------------------------------------------------------------------
    // Methods
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

        m_visitedMessages.Add(m);
        m_lastVisitedMessage = m_visitedMessages.Count - 1;
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
        return m_visitedMessages[m_lastVisitedMessage];
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
}