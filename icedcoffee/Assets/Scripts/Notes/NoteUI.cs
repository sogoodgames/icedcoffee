using UnityEngine;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    public NotesApp NotesApp;
    public Text Text;
    public Image Image;
    public Button Button;

    public void SetContent (NotesApp app, string text, Sprite sprite = null) {
        Text.text = text;
        if(sprite != null) {
            Image.sprite = null;
            Image.preserveAspect = false;
            Image.sprite = sprite;
            Image.preserveAspect = true;
            Image.gameObject.SetActive(true);

            Button.onClick.AddListener (
                delegate {app.OpenImageClue(sprite);}
            );
        }
    }
}