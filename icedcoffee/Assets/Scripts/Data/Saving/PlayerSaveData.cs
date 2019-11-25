using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class PlayerSaveData {
    public DateTime GameStartTime;
    public List<ClueID> FoundClues;
    public List<ChatProgressionData> ChatProgressionData;
    public List<GramPostProgressionData> GramPostProgressionData;

    // only call constructor when creating new save file
    public PlayerSaveData (
        List<ChatProgressionData> defaultChats,
        List<ClueID> defaultClues,
        List<GramPostProgressionData> defaultGramPosts,
        DateTime startTime
    ) {
        GameStartTime = startTime;  
        FoundClues = defaultClues;
        ChatProgressionData = defaultChats;
        GramPostProgressionData = defaultGramPosts;
        Debug.Log("Created new save file with game start time: " + GameStartTime.ToString());
    }
}