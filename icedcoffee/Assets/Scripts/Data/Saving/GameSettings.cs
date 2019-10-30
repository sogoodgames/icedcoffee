using System;
using UnityEngine;

[Serializable]
public class GameSettings {
    public string PronounPersonalSubject;
    public string PronounPersonalObject;
    public string PronounPossessive;
    public string Name;

    // only call constructor when creating new save file
    public GameSettings (
        string prounounSubj,
        string prounounObj,
        string prounounPos,
        string name
    ) {
        PronounPersonalSubject = prounounSubj;
        PronounPersonalObject = prounounObj;
        PronounPossessive = prounounPos;
        Name = name;
        Debug.Log("Created new game settings save file with player name: " 
                    + name);
    }
}