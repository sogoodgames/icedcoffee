using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public MainMenu MainMenu;
    public InputField PronounsSubj;
    public InputField PronounsObj;
    public InputField PronounsPos;
    public InputField Name;
    public Text ExampleText;
    private string Example = "[name] and [their] dog went to the park. [they-c] had a great time.";

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void Start () {
        PronounsSubj.onEndEdit.AddListener(delegate {HandleSettingsChanged();});
        PronounsObj.onEndEdit.AddListener(delegate {HandleSettingsChanged();});
        PronounsPos.onEndEdit.AddListener(delegate {HandleSettingsChanged();});
        Name.onEndEdit.AddListener(delegate {HandleSettingsChanged();});
    }

    // ------------------------------------------------------------------------
    public override void Open() {
        base.Open();

        // create new settings object if a save file isn't found
        // this is ok because we create settings before starting a new game
        if(PhoneOS.Settings == null) {
            // don't overwrite existing settings just in case
            PhoneOS.CreateSettings(save:false);
        }

        PhoneOS.LoadSettings();

        PronounsSubj.text = PhoneOS.Settings.PronounPersonalSubject;
        PronounsObj.text = PhoneOS.Settings.PronounPersonalObject;
        PronounsPos.text = PhoneOS.Settings.PronounPossessive;
        Name.text = PhoneOS.Settings.Name;

        ReplaceExample();
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
    public void ReturnToMainMenu () {
        PhoneOS.OpenApp(MainMenu);
    }

    // ------------------------------------------------------------------------
    private void HandleSettingsChanged () {
        PhoneOS.Settings.PronounPersonalSubject = PronounsSubj.text;
        PhoneOS.Settings.PronounPersonalObject = PronounsObj.text;
        PhoneOS.Settings.PronounPossessive = PronounsPos.text;
        PhoneOS.Settings.Name = Name.text;
        PhoneOS.SaveSettings();

        ReplaceExample();
    }

    // ------------------------------------------------------------------------ 
    private void ReplaceExample () {
        ExampleText.text = DialogueProcesser.PreprocessDialogue(
            Example,
            PhoneOS.Settings.Name,
            PhoneOS.Settings.PronounPersonalSubject,
            PhoneOS.Settings.PronounPersonalObject,
            PhoneOS.Settings.PronounPossessive
        );
    }
}