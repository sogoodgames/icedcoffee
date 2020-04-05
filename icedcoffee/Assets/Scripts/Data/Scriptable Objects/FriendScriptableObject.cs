using System;

using UnityEngine;

public enum FriendLevel {
    Min = 0,
    Neutral = 250, 
    Friendly = 500,
    Romantic = 750,
    Max = 100
}

[Serializable]
public class FriendProgressionData {
    public int Status;

    public FriendProgressionData (int status) {
        Status = status;
    }
}

[CreateAssetMenu(fileName = "FriendData", menuName = "IcedCoffee/ScriptableObjects/Friend", order = 1)]
public class FriendScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend Friend;
    public string Name;
    public ClueID ContactClue;
    public Sprite Icon;

    private FriendProgressionData m_progressionData;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public FriendLevel Status {
        get {return (FriendLevel)m_progressionData.Status;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void ModStatus (int mod) {
        m_progressionData.Status += mod;
    }
}
