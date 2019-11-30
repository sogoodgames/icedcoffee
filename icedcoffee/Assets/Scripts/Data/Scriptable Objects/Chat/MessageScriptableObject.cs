using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public struct MessageProgressionData {
    public int Node;
    public int OptionSelection;
    public bool MadeSelection;
    public bool IsClueMessage;

    public MessageProgressionData (
        int node,
        int selection,
        bool madeSelection,
        bool clueMessage
    ) {
        Node = node;
        OptionSelection = selection;
        MadeSelection = madeSelection;
        IsClueMessage = clueMessage;
    }
}

[CreateAssetMenu(fileName = "MessageData", menuName = "IcedCoffee/MessageScriptableObject", order = 1)]
public class MessageScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend Sender; // who sent this message
    public bool IsClueMessage; // if this is sent when presenting a clue
    public bool IsLeafMessage; // if this message has no branches
    public ClueID ClueGiven; // the clue given (if any)
    public ClueID ClueTrigger; // the clue that instigates this message
    public string[] Messages; // the text for the messages sent
    public PhotoID Image;

    // all of the following map by index
    public string[] Options; // the text options
    public int[] Branch; // the next message (-1 means this is a leaf)

    // auto-generated ID
    [SerializeField]
    private int m_node;
    public int Node {get{return m_node;}}

    // progression data
    private MessageProgressionData m_progressionData;
    
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // public progression data accessors
    public int OptionSelection {get{return m_progressionData.OptionSelection;}}
    public bool MadeSelection {get{return m_progressionData.MadeSelection;}}
    public bool HasOptions {get{return Options != null && Options.Length > 0;}}

    public bool Player {get{return Sender == Friend.You;}}

    // ------------------------------------------------------------------------
    // Debug Properties
    // ------------------------------------------------------------------------
#if UNITY_EDITOR
    // auto-filled parent chat (for validation/tool purposes)
    public ChatScriptableObject Chat;

    public int GetIndexInChat () {
        Assert.IsNotNull(Chat, "Message " + Node + " chat not set.");
        int index = -1;
        for(int i = 0; i < Chat.Messages.Length; i++) {
            if(Chat.Messages[i].Node == Node) {
                index = i;
                break;
            }
        }
        Assert.IsFalse(
            index == -1, 
            "Message" + Node + " not found in chat " + Chat.ID
        );
        return index;
    }
#endif

#if DEBUG
    public string DebugName {
        get {
            string name = "[" + Node + "]";
            string msg = "";
            if(Player && Options != null && Options.Length > 0) {
                msg = Options[0];
            } else if(Messages != null && Messages.Length > 0){
                msg = Messages[0];
            }

            if(!string.IsNullOrEmpty(msg)) {
                int lastIndex = msg.Length < 20 ? msg.Length : 20; 
                name += "- " + msg.Substring(0, lastIndex) + "...";
            }
            return name;
        }
    }
#endif

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void ClearProgression () {
        m_progressionData = new MessageProgressionData(
            m_node,
            0,
            false,
            IsClueMessage
        );
    }

    // ------------------------------------------------------------------------
    public void SelectOption (int option) {
        m_progressionData.MadeSelection = true;
        m_progressionData.OptionSelection = option;
    }

    // ------------------------------------------------------------------------
    public void LoadProgression (MessageProgressionData data) {
        m_progressionData = data;
    }
}