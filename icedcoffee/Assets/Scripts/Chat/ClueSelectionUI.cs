using UnityEngine;

public class ClueSelectionUI : ChatSelectionUI
{
    public PhoneOS PhoneOS;
    public Transform ClueListParent;
    public GameObject ClueUIPrefab;
    public ChatRunner ChatRunner;
    public ChatApp ChatApp;

    public override void Open (Chat chat) {
        // populate clues that you can send in a chat
        foreach(Clue clue in PhoneOS.UnlockedClues) {
            // don't display clues that we can't send in chats
            // or clues we've already visited
            // or clues with an image (they'll be in the image selection UI)
            if(clue.ClueID == ClueID.NoClue 
               || !clue.CanSend
               || chat.presentedClues.Contains(clue.ClueID)
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

        gameObject.SetActive(true);
    }

    public override void Close () {
        ChatApp.LineImage.SetActive(true);
        ChatApp.EnterMessageButtons.Open();
        foreach(Transform t in ClueListParent.transform) {
            Destroy(t.gameObject);
        }
        gameObject.SetActive(false);
    }
}
