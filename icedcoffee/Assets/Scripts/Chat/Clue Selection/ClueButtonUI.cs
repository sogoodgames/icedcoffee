using UnityEngine;
using UnityEngine.UI;

public class ClueButtonUI : MonoBehaviour
{
    public Text Text;
    public Button Button;

    public void Init (Clue clue, ChatRunner chatRunner) {
        Text.text = clue.Note;
        Button.onClick.AddListener ( 
            delegate { chatRunner.SelectClueOption(clue.ClueID); }
        );
    }
}
