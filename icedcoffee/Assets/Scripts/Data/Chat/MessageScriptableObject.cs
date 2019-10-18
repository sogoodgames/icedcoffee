using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct MessageProgressionData {
    public int Node;
    public int OptionSelection;
    public bool MadeSelection;

    public MessageProgressionData (int node, int selection, bool madeSelection) {
        Node = node;
        OptionSelection = selection;
        MadeSelection = madeSelection;
    }
}

[CreateAssetMenu(fileName = "MessageData", menuName = "IcedCoffee/MessageScriptableObject", order = 1)]
public class MessageScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public int Node; // the ID for this message
    public bool Player; // whether or not it's the player talking
    public ClueID ClueGiven; // the clue given (if any)
    public ClueID ClueTrigger; // the clue that instigates this message
    public string[] Messages; // the text for the messages sent
    public PhotoID Image;

    // all of the following map by index
    public string[] Options; // the text options
    public int[] Branch; // the next message (-1 means this is a leaf)

    // progression data
    private MessageProgressionData m_progressionData;
    
    // public progression data accessors
    public int OptionSelection {get{return m_progressionData.OptionSelection;}}
    public bool MadeSelection {get{return m_progressionData.MadeSelection;}}

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void ClearProgression () {
        m_progressionData = new MessageProgressionData(Node, 0, false);
    }

    // ------------------------------------------------------------------------
    public void SelectOption (int option) {
        m_progressionData.MadeSelection = true;
        m_progressionData.OptionSelection = option;
    }

    // ------------------------------------------------------------------------
    public bool HasOptions () {
        return Options != null && Options.Length > 0;
    }

    // ------------------------------------------------------------------------
    public void LoadProgression (MessageProgressionData data) {
        m_progressionData = data;
    }
}