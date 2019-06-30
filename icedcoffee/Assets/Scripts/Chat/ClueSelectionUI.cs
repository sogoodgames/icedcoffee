using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueSelectionUI : MonoBehaviour
{
    public PhoneOS PhoneOS;
    public Transform ClueListParent;
    public GameObject ClueUIPrefab;
    public ChatRunner ChatRunner;

    public void Open (Chat chat) {
        // populate clues that you can send in a chat
        foreach(Clue clue in PhoneOS.UnlockedClues) {
            // don't display clues that we can't send in chats
            // or clues we've already visited
            if(clue.ClueID == ClueID.NoClue 
               || !clue.CanSend
               || chat.presentedClues.Contains(clue.ClueID)
            ) {
                continue;
            }

            GameObject buttonObj = Instantiate(ClueUIPrefab, ClueListParent);
            ClueButtonUI buttonUI = buttonObj.GetComponent<ClueButtonUI>();
            if(buttonUI) {
                buttonUI.Init(clue, ChatRunner);
            }
        }

        gameObject.SetActive(true);
    }

    public void Close () {
        foreach(Transform t in ClueListParent.transform) {
            Destroy(t.gameObject);
        }
        gameObject.SetActive(false);
    }
}
