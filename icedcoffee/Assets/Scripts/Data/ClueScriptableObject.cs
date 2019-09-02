using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "IcedCoffee/ClueScriptableObject", order = 1)]
public class ClueScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public bool Unlocked;
    public bool CanSend; // can be presented in a chat
    public ClueID ClueID;
    public Friend PhoneNumberGiven;
    public string Note;
    public MessageScriptableObject Message;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public bool Invalid {
        get{return string.IsNullOrEmpty(Note);}
    }
}