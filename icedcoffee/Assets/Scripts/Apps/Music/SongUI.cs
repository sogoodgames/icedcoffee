using UnityEngine;
using UnityEngine.UI;

public class SongUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Text TitleText;
    public Text ArtistAlbumText;
    public Button PlayButton;

    private SongScriptableObject m_song;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetSongContent(SongScriptableObject song) {
        m_song = song;
        TitleText.text = m_song.Title;
        ArtistAlbumText.text = m_song.Artist + " // " + m_song.Album;
    }
}