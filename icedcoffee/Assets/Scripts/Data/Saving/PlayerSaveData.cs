using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class PlayerSaveData {
    public DateTime GameStartTime;
    public List<ClueID> FoundClues;

    public PlayerSaveData () {
        GameStartTime = DateTime.Now;
        FoundClues = new List<ClueID>();
        Debug.Log("Created new save file with game start time: " + GameStartTime.ToString());
    }
}