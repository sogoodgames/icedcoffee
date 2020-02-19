using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    public NotesApp NotesApp;
    public Text Text;
    public PhotoTile PhotoTile;
    public Button Button;

    public void SetContent (NotesApp app, string text, Sprite sprite = null) {
        if(PhotoTile == null || PhotoTile.Image == null) {
            Assert.IsTrue(PhotoTile != null && PhotoTile.Image != null);
            return;
        }

        Text.text = text;
        if(sprite != null) {
            PhotoTile.Image.sprite = null;
            PhotoTile.Image.preserveAspect = false;
            PhotoTile.Image.sprite = sprite;
            PhotoTile.Image.preserveAspect = true;
            PhotoTile.gameObject.SetActive(true);

            Button.onClick.AddListener (
                delegate {app.OpenImageClue(sprite);}
            );
        } else {
            PhotoTile.gameObject.SetActive(false);
        }
    }
}