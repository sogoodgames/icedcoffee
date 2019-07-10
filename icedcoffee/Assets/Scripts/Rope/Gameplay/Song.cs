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

    private MusicUserId m_userID;
    public MusicUserId UserID {
        get {return m_userID;}
    }

    private Friend m_friendID;
    public Friend FriendID {
        get {return m_friendID;}
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
        m_userID = user.userID;
        m_friendID = user.friendID;

        m_playlist = new List<Song>();
        foreach(SongSerializable song in user.playlist) {
            m_playlist.Add(new Song(song));
        }
    }
}