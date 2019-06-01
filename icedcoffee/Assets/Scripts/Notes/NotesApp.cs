using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesApp : App
{
    // game
    public Transform NotesParent;
    public GameObject NotePrefab;

    public override void Open() {
        base.Open();
        PopulateNotes(); 
    }

    public override void OnCloseAnimationFinished () {
        base.OnCloseAnimationFinished();
        foreach(Transform child in NotesParent.transform) {
            Destroy(child.gameObject);
        }
    }

    private void PopulateNotes () {
        foreach(Clue clue in PhoneOS.UnlockedClues) {
            if(clue.ClueID == ClueID.NoClue || clue.PhoneNumberGiven != Friend.NoFriend) {
                continue;
            }

            GameObject noteObj = Instantiate(NotePrefab, NotesParent);
            NoteUI noteUI = noteObj.GetComponent<NoteUI>();
            if(noteUI) {
                noteUI.Text.text = clue.Note;
            }
        }
    }
}
