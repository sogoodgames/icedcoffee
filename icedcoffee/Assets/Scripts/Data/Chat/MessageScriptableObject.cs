using UnityEngine;
using System.Collections.Generic;

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

    // the selected option (if any)
    [HideInInspector]
    public int OptionSelection;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void OnEnable () {
        OptionSelection = -1;
    }

    // ------------------------------------------------------------------------
    public bool MadeSelection () {
        if(!HasOptions()) return true;
        return OptionSelection >= 0;
    }

    // ------------------------------------------------------------------------
    public bool HasOptions () {
        return Options != null && Options.Length > 0;
    }
}