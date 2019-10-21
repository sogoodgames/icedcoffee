using UnityEngine;

public class ImageSelectionUI : ChatSelectionUI
{
    public PhoneOS PhoneOS;
    public Transform ImageListParent;
    public GameObject ImageTilePrefab;
    public ChatRunner chatRunner;

    public override void Open (ChatScriptableObject chat) {
        ClearButtons();
        
        foreach(PhotoScriptableObject photo in PhoneOS.FoundPhotos) {
            ClueID clue = photo.ClueID;
            // don't display clues we've already visisted
            if(clue == ClueID.NoClue || chat.PresentedClues.Contains(clue)) {
                continue;
            }

            GameObject imageTile = Instantiate (ImageTilePrefab, ImageListParent);
            ImageButtonUI imageButtonUI = imageTile.GetComponent<ImageButtonUI>();
            if(imageButtonUI) {
                imageButtonUI.Init(
                    PhoneOS.GameData.GetClue(clue),
                    chatRunner,
                    photo.Image
                );
            }
        }

        gameObject.SetActive(true);
    }

    public override void Close () {
        ClearButtons();
        gameObject.SetActive(false);
    }

    private void ClearButtons () {
        foreach(Transform t in ImageListParent.transform) {
            Destroy(t.gameObject);
        }
    }
}
