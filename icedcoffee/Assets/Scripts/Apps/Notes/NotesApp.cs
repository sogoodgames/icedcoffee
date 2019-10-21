using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Transform NotesParent;
    public GameObject NotePrefab;
    public FullscreenImage FullscreenImage;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open() {
        base.Open();
        PopulateNotes(); 
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            foreach(Transform child in NotesParent.transform) {
                Destroy(child.gameObject);
            }
        }
        base.HandleSlideAnimationFinished();
    }

    // ------------------------------------------------------------------------
    public void OpenImageClue (Sprite sprite) {
        FullscreenImage.Open(sprite);
    }

    // ------------------------------------------------------------------------
    private void PopulateNotes () {
        foreach(ClueScriptableObject clue in PhoneOS.UnlockedClues) {
            if(clue.ClueID == ClueID.NoClue || clue.PhoneNumberGiven != Friend.NoFriend) {
                continue;
            }

            GameObject noteObj = Instantiate(NotePrefab, NotesParent);
            NoteUI noteUI = noteObj.GetComponent<NoteUI>();
            if(noteUI) {
                PhotoScriptableObject photo = PhoneOS.GameData.GetPhoto(clue.ClueID);
                if(photo != null) {
                    Sprite sprite = photo.Image;
                    noteUI.SetContent(this, clue.Note, sprite);
                } else {
                    noteUI.SetContent(this, clue.Note);
                }
            }
        }
    }
}
