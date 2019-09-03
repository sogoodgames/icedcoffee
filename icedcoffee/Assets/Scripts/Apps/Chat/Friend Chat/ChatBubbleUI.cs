using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleUI : MonoBehaviour {
    public Text Text;
    public Image Icon;
    public Button Button;

    // ------------------------------------------------------------------------
    public void AddAttachment (string message, PhotoID photoID, ChatApp chatApp) {
        Text.text = message + " [click to open attachment]";
        Button.onClick.AddListener(
            delegate {chatApp.OpenAttachment(photoID);}
        );
        Button.interactable = true;
    }
}