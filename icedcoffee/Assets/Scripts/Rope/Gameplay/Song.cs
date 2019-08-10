using System;   // serializable
using System.Collections.Generic;

public class Song {
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private string m_title;
    public string Title {
        get {return m_title;}
    }

    private string m_artist;
    public string Artist {
        get {return m_artist;}
    }

    private string m_album;
    public string Album {
        get {return m_album;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public Song (SongSerializable song) {
        m_title = song.title;
        m_album = song.album;
        m_artist = song.artist;
    }
}

public class MusicUser {
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private bool m_isPlayer;
    public bool IsPlayer {
        get {return m_isPlayer;}
    }

    private string m_username;
    public string Username {
        get {return m_username;}
    }

    private string m_playlistName;
    public string PlaylistName {
        get {return m_playlistName;}
    }

    private int m_numFollowers;
    public int NumFollowers {
        get {return m_numFollowers;}
    }

    private Friend m_friendID;
    public Friend FriendID {
        get {return m_friendID;}
    }

    private ClueID m_clueGiven;
    public ClueID ClueGiven {
        get {return m_clueGiven;}
    }

    private ClueID m_clueNeeded;
    public ClueID ClueNeeded {
        get {return m_clueNeeded;}
    }

    private List<Song> m_playlist;
    public List<Song> Playlist {
        get {return m_playlist;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public MusicUser (MusicUserSerializable user) {
        m_isPlayer = user.isPlayer;
        m_username = user.username;
        m_playlistName = user.playlistName;
        m_friendID = user.friendID;
        m_numFollowers = user.numFollowers;
        m_clueGiven = user.clueGiven;
        m_clueNeeded = user.clueNeeded;

        m_playlist = new List<Song>();
        foreach(SongSerializable song in user.playlist) {
            m_playlist.Add(new Song(song));
        }
    }
}