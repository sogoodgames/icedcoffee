using UnityEngine;

public class MainMenu : App
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open() {
        Debug.Log("called main menu open");
        base.Open();
    }

    // ------------------------------------------------------------------------
    public override void Close() { 
        base.Close();
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------
    public void StartNewGame () {
        PhoneOS.StartNewGame();
        PhoneOS.GoHome();
    }

    // ------------------------------------------------------------------------
    public void LoadGame () {
        PhoneOS.LoadGame();
        PhoneOS.GoHome();
    }
}
