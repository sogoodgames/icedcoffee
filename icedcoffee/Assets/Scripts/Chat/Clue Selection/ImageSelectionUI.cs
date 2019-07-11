using UnityEngine;

public class ImageSelectionUI : ChatSelectionUI
{
    public PhoneOS PhoneOS;
    public Transform ImageListParent;
    public GameObject ImageTilePrefab;
    public ChatRunner chatRunner;

    public override void Open (Chat chat) {
        ClearButtons();
        
        foreach(Photo photo in PhoneOS.FoundPhotos) {
            ClueID clue = photo.ClueID;
            // don't display clues we've already visisted
            if(clue == ClueID.NoClue || chat.presentedClues.Contains(clue)) {
                continue;
            }

            GameObject imageTile = Instantiate (ImageTilePrefab, ImageListParent);
            ImageButtonUI imageButtonUI = imageTile.GetComponent<ImageButtonUI>();
            if(imageButtonUI) {
                imageButtonUI.Init(
                    PhoneOS.GetClue(clue),
                    chatRunner,
                    PhoneOS.GetIcon(photo.Image)
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
