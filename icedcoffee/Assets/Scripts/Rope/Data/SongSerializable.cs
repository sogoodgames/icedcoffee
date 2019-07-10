using System;   // serializable

[Serializable]
public class SongSerializable {
    public string title;
    public string artist;
    public string album;
}

[Serializable] 
public class MusicUserSerializable {
    public bool isPlayer;
    public string username;
    public MusicUserId userID;
    public Friend friendID;
    public SongSerializable[] playlist;
}