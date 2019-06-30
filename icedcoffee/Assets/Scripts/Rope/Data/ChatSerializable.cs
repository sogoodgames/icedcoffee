using System;   // serializable
using System.Collections.Generic;

[Serializable]
public class MessageSerializable {
    public int node; // the ID for this message
    public bool player; // whether or not it's the player talking
    public ClueID clueGiven; // the clue given (if any)
    public ClueID clueTrigger; // the clue that instigates this message
    public string[] messages; // the text for the messages sent
    public PhotoID image;

    // all of the following map by index
    public string[] options; // the text options
    public int[] branch; // the next message (-1 means this is a leaf)
}

[Serializable]
public class ChatSerializable {
    public Friend friend; // the person you're talking to
    public int icon; // icon file
    public ClueID clueNeeded; // the clue needed to unlock the chat
    public MessageSerializable[] messages; // all of the messages
}