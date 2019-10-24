using UnityEngine;
using System;
using System.Collections.Generic;

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
    public bool Player; // whether or not it's the player talking
    public bool IsClueMessage; // if this is a message sent when presenting a clue 
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
    
    // public progression data accessors
    public int OptionSelection {get{return m_progressionData.OptionSelection;}}
    public bool MadeSelection {get{return m_progressionData.MadeSelection;}}
    public bool HasOptions {get{return Options != null && Options.Length > 0;}}
    public bool HasBranch {get{return Branch != null && Branch.Length > 0;}}

#if UNITY_EDITOR
    // auto-filled parent chat (for validation/tool purposes)
    public ChatScriptableObject Chat;
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