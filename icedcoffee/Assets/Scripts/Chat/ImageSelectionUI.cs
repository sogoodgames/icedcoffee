using UnityEngine;

public class ImageSelectionUI : ChatSelectionUI
{
    public PhoneOS PhoneOS;
    public Transform ImageListParent;
    public GameObject ImageTilePrefab;
    public ChatRunner chatRunner;
    public ChatApp ChatApp;

    public override void Open (Chat chat) {
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
        ChatApp.LineImage.SetActive(true);
        ChatApp.EnterMessageButtons.Open();
        foreach(Transform t in ImageListParent.transform) {
            Destroy(t.gameObject);
        }
        gameObject.SetActive(false);
    }
}
