using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueSelectionUI : MonoBehaviour
{
    public PhoneOS PhoneOS;
    public Transform ClueListParent;
    public GameObject ClueUIPrefab;

    void OnEnable () {
        Debug.Log("meep");
        // populate clues that you can send in a chat
        foreach(Clue clue in PhoneOS.UnlockedClues) {
            if(clue.ClueID == ClueID.NoClue || !clue.CanSend) {
                continue;
            }
            Debug.Log("Adding clue: " + clue.ClueID);

            GameObject buttonObj = Instantiate(ClueUIPrefab, ClueListParent);
            ClueButtonUI buttonUI = buttonObj.GetComponent<ClueButtonUI>();
            if(buttonUI) {
                buttonUI.Init(clue);
            }
        }
    }
}
