using UnityEngine;
using UnityEngine.UI;

public class ImageButtonUI : MonoBehaviour
{
    public Image Image;
    public Button Button;

    public void Init (Clue clue, ChatRunner chatRunner, Sprite sprite) {
        Image.sprite = sprite;
        Button.onClick.AddListener ( 
            delegate { chatRunner.SelectClueOption(clue); }
        );
    }
}
