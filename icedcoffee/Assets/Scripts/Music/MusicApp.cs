using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicApp : App {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PlaylistUI PlaylistUI;
    public FriendListUI FriendListUI;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open () {
        base.Open();
        OpenPlayerPlaylist();
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            PlaylistUI.Close();
            FriendListUI.Close();
        }
        base.HandleSlideAnimationFinished();
    }

    // ------------------------------------------------------------------------
    public void OpenPlayerPlaylist () {
        FriendListUI.Close();
        PlaylistUI.Open(MusicUserId.You);
    }

    // ------------------------------------------------------------------------
    public void OpenPlaylist (MusicUserId id) {
        FriendListUI.Close();
        PlaylistUI.Open(id);
    }

    // ------------------------------------------------------------------------
    public void OpenFriendList () {
        PlaylistUI.Close();
        FriendListUI.Open();
    }
}