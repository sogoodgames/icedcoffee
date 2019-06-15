using UnityEngine;
using UnityEngine.UI;

public class ClueButtonUI : MonoBehaviour
{
    public Text Text;
    public Button Button;

    private ClueID clueId;

    public void Init (Clue clue) {
        Text.text = clue.Note;
        clueId = clue.ClueID;
    }
}
