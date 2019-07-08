using System;   // serializable

[Serializable]
public class ClueSerializable {
    public bool unlocked;
    public bool canSend; // can be presented in a chat
    public ClueID clueID;
    public Friend phoneNumberGiven;
    public string note;
    public MessageSerializable message;
}