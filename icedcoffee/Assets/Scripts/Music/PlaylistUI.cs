using UnityEngine;
using UnityEngine.UI;

public class PlaylistUI : MonoBehaviour 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PhoneOS PhoneOS;
    public Text PlaylistTitleText;
    public Text UsernameTitleText;
    public Transform PlaylistParent;
    public GameObject SongPrefab;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Open (Friend id) {
        MusicUserScriptableObject user = PhoneOS.GetMusicUser(id);

        // set UI text for username, playlist name
        UsernameTitleText.text = "by " + user.Username + " // " + user.NumFollowers + " followers";
        PlaylistTitleText.text = user.PlaylistName;

        // populate list of songs
        ClearSongList();
        foreach(SongScriptableObject song in user.Playlist) {
            GameObject songObj = Instantiate (
                SongPrefab,
                PlaylistParent
            );

            SongUI songUI = songObj.GetComponent<SongUI>();
            songUI.SetSongContent(song.Title, song.Artist, song.Album);
        }

        gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public void Close () {
        ClearSongList();
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------
    private void ClearSongList () {
        foreach(Transform child in PlaylistParent.transform) {
            Destroy(child.gameObject);
        }
    }
}