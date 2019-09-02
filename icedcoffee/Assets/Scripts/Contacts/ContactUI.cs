using UnityEngine;
using UnityEngine.UI;

public class ContactUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Text NameText;
    public Text GramText;
    public Text MusicText;
    public Text ForumText;
    public Image Image;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetContent (
        string name,
        string gram,
        string music,
        string forum,
        Sprite icon
    ) {
        NameText.text = name;
        GramText.text = gram;
        MusicText.text = music;
        ForumText.text = forum;
        Image.sprite = icon;
    }
}
