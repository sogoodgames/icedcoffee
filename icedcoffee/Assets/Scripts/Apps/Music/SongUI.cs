using UnityEngine;
using UnityEngine.UI;

public class SongUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Text TitleText;
    public Text ArtistAlbumText;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetSongContent(string title, string artist, string album) {
        TitleText.text = title;
        ArtistAlbumText.text = artist + " // " + album;
    }
}