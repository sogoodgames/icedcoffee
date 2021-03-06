using UnityEngine;

[CreateAssetMenu(fileName = "ClueData", menuName = "IcedCoffee/ScriptableObjects/Clue", order = 1)]
public class ClueScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public bool InitialLockState;
    public bool CanSend; // can be presented in a chat
    public ClueID ClueID;
    public Friend PhoneNumberGiven;
    public string Note;
    public MessageScriptableObject Message;

    [HideInInspector]
    public bool Unlocked;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public bool Invalid {
        get{return string.IsNullOrEmpty(Note);}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void OnEnable () {
        Unlocked = InitialLockState;
    }
}