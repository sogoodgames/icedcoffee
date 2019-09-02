using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(InputField))]
public class ClueSearchUI : MonoBehaviour 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public InputField InputField;
    public ChatApp ChatApp;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void Start () {
        InputField.onValueChanged.AddListener(delegate {EditSearchString();});
    }

    // ------------------------------------------------------------------------
    void Update () {
        if(InputField.isFocused && !ChatApp.ClueSelectionUI.IsOpen) {
            ChatApp.OpenClueSelection();
        }
    }

    // ------------------------------------------------------------------------
    public void EditSearchString () {
        // if our search field is empty, return all unlocked clues
        if(string.IsNullOrEmpty(InputField.text)) {
            ChatApp.ClueSelectionUI.CreateButtons(ChatApp.PhoneOS.UnlockedClues);
            return;
        }

        // search clues by 'note' field for string match
        List<ClueScriptableObject> clues = new List<ClueScriptableObject>();
        foreach(ClueScriptableObject clue in ChatApp.PhoneOS.UnlockedClues) {
            if(clue.Note.Contains(InputField.text)) {
                clues.Add(clue);
            }
        }
        ChatApp.ClueSelectionUI.CreateButtons(clues);
    }
}