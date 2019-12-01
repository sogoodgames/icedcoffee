using System;

using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleUI : MonoBehaviour {
    public Text Text;
    public Text TimeText;
    public Image Icon;
    public Button Button;

    // ------------------------------------------------------------------------
    public void Setup (
        string message,
        Sprite icon,
        DateTime time
    ) {
        Text.text = message;
        Icon.sprite = icon;
        TimeText.text = DialogueProcesser.FormatDateTime(time);
    }

    // ------------------------------------------------------------------------
    public void AddAttachment (
        string message,
        PhotoID photoID,
        ChatApp chatApp
    ) {
        Text.text = message + " [click to open attachment]";
        Button.onClick.AddListener(
            delegate {chatApp.OpenAttachment(photoID);}
        );
        Button.interactable = true;
    }
}