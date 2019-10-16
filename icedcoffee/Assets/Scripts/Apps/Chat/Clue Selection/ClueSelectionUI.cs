﻿using UnityEngine;
using System.Collections.Generic;

public class ClueSelectionUI : ChatSelectionUI
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PhoneOS PhoneOS;
    public Transform ClueListParent;
    public GameObject ClueUIPrefab;
    public ChatRunner ChatRunner;

    private ChatScriptableObject chat;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open (ChatScriptableObject chat) {
        gameObject.SetActive(true);
        this.chat = chat;
        CreateButtons(PhoneOS.UnlockedClues);
    }

    // ------------------------------------------------------------------------
    public override void Close () {
        ClearButtons();
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------
    public void CreateButtons(List<ClueScriptableObject> clues) {
        if(chat == null) {
            return;
        }

        ClearButtons();
        // populate clues that you can send in a chat
        foreach(ClueScriptableObject clue in clues) {
            // don't display clues that we can't send in chats
            // or clues we've already visited
            // or clues with an image (they'll be in the image selection UI)
            if(clue.ClueID == ClueID.NoClue 
               || !clue.CanSend
               || chat.PresentedClues.Contains(clue.ClueID)
               || PhoneOS.GetPhoto(clue.ClueID) != null
            ) {
                continue;
            }

            GameObject buttonObj = Instantiate(ClueUIPrefab, ClueListParent);
            ClueButtonUI buttonUI = buttonObj.GetComponent<ClueButtonUI>();
            if(buttonUI) {
                buttonUI.Init(clue, ChatRunner);
            }
        }
    }

    // ------------------------------------------------------------------------
    private void ClearButtons() {
        foreach(Transform t in ClueListParent.transform) {
            Destroy(t.gameObject);
        }
    }
}