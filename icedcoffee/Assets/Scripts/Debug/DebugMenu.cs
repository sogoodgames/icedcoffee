using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    // PUBLIC REFERENCES TO EVERYTHING
    // CAUSE THIS IS DEBUG AND IDGAF
    public PhoneOS PhoneOS;
    public ChatRunner ChatRunner;
    public InputField ClueInputField; 

    private float m_cachedMessageDelay;

    private ClueID m_clueToToggle;

    public void SetClueToToggle (string clue) {
        int clueInt = Int32.Parse(clue);
        m_clueToToggle = (ClueID)clueInt;
        Debug.Log("Clue: " + m_clueToToggle);
    }

    void OnEnable () {
        m_cachedMessageDelay = ChatRunner.MaxTimeBetweenMessages;
    }

    public void ToggleOpen () {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToggleInstantChat () {
        if(ChatRunner.MaxTimeBetweenMessages < 0.001) {
            ChatRunner.MaxTimeBetweenMessages = m_cachedMessageDelay;
        } else {
            ChatRunner.MaxTimeBetweenMessages = 0;
        }
    }

    public void ToggleClue () {
        string input = ClueInputField.text;
        PhoneOS.DebugToggleClue(input);
    }
}
