using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class PlayerSaveData {
    public DateTime GameStartTime;
    public List<ClueID> FoundClues;
    public List<ChatProgressionData> ChatProgressionData;

    // only call constructor when creating new save file
    public PlayerSaveData (
        List<ChatProgressionData> defaultChats,
        List<ClueID> defaultClues,
        DateTime startTime
    ) {
        GameStartTime = startTime;  
        FoundClues = defaultClues;
        ChatProgressionData = defaultChats;
        Debug.Log("Created new save file with game start time: " + GameStartTime.ToString());
    }
}