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
    public string playlistName;
    public int numFollowers;
    public Friend friendID;
    public ClueID clueNeeded;
    public ClueID clueGiven;
    public SongSerializable[] playlist;
}