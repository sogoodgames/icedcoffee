using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class PlayerSaveData {
    public DateTime GameStartTime;
    public List<ClueID> FoundClues;
    public List<ChatProgressionData> ChatProgressionData;

    // only call constructor when creating new save file
    public PlayerSaveData () {
        GameStartTime = DateTime.Now;  
        FoundClues = new List<ClueID>();
        ChatProgressionData = new List<ChatProgressionData>();
        Debug.Log("Created new save file with game start time: " + GameStartTime.ToString());
    }
}