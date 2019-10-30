using UnityEngine;
using UnityEngine.UI;

public class MainMenu : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Button LoadButton;
    public Button SaveButton;
    public SettingsMenu SettingsMenu;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open() {
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
    private void OnEnable () {
        SetupButtons();
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

    // ------------------------------------------------------------------------
    public void Exit () {
        Application.Quit();
    }

    // ------------------------------------------------------------------------
    public void OpenSettings () {
        PhoneOS.OpenApp(SettingsMenu);
    }

    // ------------------------------------------------------------------------
    private void SetupButtons () {
        if(PhoneOS.CanLoadSaveFile) {
            LoadButton.interactable = true;
        } else {
            LoadButton.interactable = false;
        }
    }
}
