#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
#define DEBUG
#endif

using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;
using System.IO;
using System.Linq;

// Handles app opening/ closing,
// clue tracking,
// and saving
public class PhoneOS : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public bool RunFTUE = true;

    public GameData GameData;
    public ChatRunner ChatRunner;
    public NotificationManager NotificationManager;
    public NotesApp NotesApp;
    public ChatApp ChatApp;
    public GameObject ReturnButton;
    public List<App> Apps;
    public App HomeApp;
    public App MainMenuApp;

    private SaveDataLoader SaveDataLoader;
    private App m_activeApp;

    private bool m_initialized = false;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public GameSettings Settings {get {return SaveDataLoader.Settings;}}

    public bool CanLoadSaveFile {
        get { 
            if(SaveDataLoader == null) {
                SaveDataLoader = new SaveDataLoader();
            }
            return SaveDataLoader.CanLoadSaveData;
        }
    }

    public List<FriendScriptableObject> ActiveFriends {
        get {
            List<FriendScriptableObject> activeFriends = new List<FriendScriptableObject>();
            foreach(FriendScriptableObject f in GameData.Friends) {
                if(ClueRequirementMet(f.ContactClue)) {
                    activeFriends.Add(f);
                }
            }
            return activeFriends;
        }
    }

    public List<ChatScriptableObject> ActiveChats {
        get {
            // only add chats that are available
            List<ChatScriptableObject> activeChats = new List<ChatScriptableObject>();
            foreach(ChatScriptableObject c in GameData.Chats) {
                //Debug.Log("checking if clue met for chat " + c.Friend + "; clue needed: " + c.ClueNeeded.ToString());
                if(ClueRequirementMet(c.ClueNeeded)) {
                    activeChats.Add(c);
                }
            }
            return activeChats;
        }
    }

    public List<ForumPostScriptableObject> AllForumPosts {
        get {return GameData.ForumPosts;}
    }

    public List<ForumPostScriptableObject> ActiveForumPosts {
        get {
            // only add posts that are available
            List<ForumPostScriptableObject> activePosts = new List<ForumPostScriptableObject>();
            foreach(ForumPostScriptableObject p in GameData.ForumPosts) {
                if(ClueRequirementMet(p.ClueNeeded)) {
                    activePosts.Add(p);
                }
            }
            return activePosts;
        }
    }

    public List<GramPostScriptableObject> ActiveGramPosts {
        get {
            List<GramPostScriptableObject> activePosts = new List<GramPostScriptableObject>();
            foreach(GramPostScriptableObject p in GameData.GramPosts) {
                if(ClueRequirementMet(p.ClueNeeded)) {
                    activePosts.Add(p);
                }
            }
            return activePosts;
        }
    }

    public List<MusicUserScriptableObject> ActiveMusicUsers {
        get {
            List<MusicUserScriptableObject> activeUsers = new List<MusicUserScriptableObject>();
            foreach(MusicUserScriptableObject user in GameData.MusicUsers) {
                if(ClueRequirementMet(user.ClueNeeded) && user.FriendID != Friend.You) {
                    activeUsers.Add(user);
                }
            }
            return activeUsers;
        }
    }

    public List<ClueScriptableObject> UnlockedClues {
        get {
            // only add unlocked clues
            List<ClueScriptableObject> clues = new List<ClueScriptableObject>();
            foreach(ClueScriptableObject clue in GameData.Clues) {
                if(ClueRequirementMet(clue.ClueID)) {
                    clues.Add(clue);
                }
            }
            return clues;
        }
    }

    public List<PhotoScriptableObject> FoundPhotos {
        get {
            // only add found photos
            List<PhotoScriptableObject> photos = new List<PhotoScriptableObject>();
            foreach(PhotoScriptableObject photo in GameData.Photos) {
                if(ClueRequirementMet(photo.ClueID)) {
                    photos.Add(photo);
                }
            }
            return photos;
        }
    }

    // ------------------------------------------------------------------------
    // Methods: Monobehaviour
    // ------------------------------------------------------------------------
    // VERY IMPORTANT for guarunteeing that all game data is available
    // upon load
    void Awake () {
        Init();
    }
    
    // ------------------------------------------------------------------------
    // Methods: Phone navigation
    // ------------------------------------------------------------------------
    public void OpenApp (App app) {
        OpenApp(app, false);
    }

    // ------------------------------------------------------------------------
    public void OpenApp (App app, bool force) {
        Assert.IsNotNull(app);

        if(app.IsOpen && !force) {
            return;
        }

        CloseAllApps();
        app.Open();
        m_activeApp = app;
    }

    // ------------------------------------------------------------------------
    public void GoHome () {
        CloseAllApps();
        HomeApp.Open();
        m_activeApp = HomeApp;
    }

    // ------------------------------------------------------------------------
    public void Return () {
        m_activeApp.Return();
    }

    // ------------------------------------------------------------------------
    // Methods: Clues
    // ------------------------------------------------------------------------
    public bool ClueRequirementMet (ClueID id) {
        ClueScriptableObject clue = GameData.GetClue(id);
        if(!clue.Invalid) {
            return clue.Unlocked;
        }
        return false;
    }

    // ------------------------------------------------------------------------
    public void FoundClue (ClueID id) {
        // don't do anything if we already knew
        if(ClueRequirementMet(id)) {
            return;
        }

        // find clue data
        ClueScriptableObject clue = GameData.GetClue(id);

        // Let notification manager know
        NotificationManager.FoundClue(clue);

        // if a photo needed this clue, mark it found
        foreach(PhotoScriptableObject photo in GameData.Photos) {
            //Debug.Log("clue id: " + photo.ClueID + "; looking for: " + id);
            if(photo.ClueID == id) {
                photo.Found = true;
            }
        }

        // Let save data manager know
        SaveDataLoader.FoundClue(id);

        // if a chat needed this clue, add it to progression data
        ChatScriptableObject chat;
        if(chat = GameData.Chats.FirstOrDefault(c => c.ClueNeeded == id)) {
            SaveDataLoader.FoundChat(chat.ProgressionData);
        }

        clue.Unlocked = true;
    }

    // ------------------------------------------------------------------------
    // Methods: App Lifecycle
    // ------------------------------------------------------------------------
    private void Init () {
        m_initialized = true;

        // set screen resolution
        Screen.SetResolution(480, 848, false);

        // init game data
        GameData.Init();

        // create save data loader
        if(SaveDataLoader == null) {
            SaveDataLoader = new SaveDataLoader();
        }

        // subscribe to game events
        ChatRunner.FoundClue += FoundClue;

        m_activeApp = MainMenuApp;
    }

    // ------------------------------------------------------------------------
    public void StartNewGame () {
        //Debug.Log("attempting start new game. init state: " + m_initialized);
        if(!m_initialized) {
            Init();
        }

        foreach(ChatScriptableObject chat in GameData.Chats) {
            chat.ClearProgression();
        }

        // create list of default found clues and chats
        List<ClueID> defaultClues = new List<ClueID>();
        foreach(ClueScriptableObject clueObj in GameData.Clues) {
            if(clueObj.InitialLockState) {
                defaultClues.Add(clueObj.ClueID);
            }
        }

        List<ChatProgressionData> defaultChats = new List<ChatProgressionData>();
        foreach(ChatScriptableObject chatObj in GameData.Chats) {
            if(ClueRequirementMet(chatObj.ClueNeeded)) {
                defaultChats.Add(chatObj.ProgressionData);
            }
        }

        SaveDataLoader.CreateNewSave(defaultChats, defaultClues);

        if(Settings == null) {
            CreateSettings(save:true);
        }

        // TODO: probably temp, will add FTUE later
        GoHome();
    }

    // ------------------------------------------------------------------------
    // button in main menu
    public void LoadGame () {
        if(!m_initialized) {
            Init();
        }

        SaveDataLoader.LoadSaveData();
        PropagateSaveData();

        if(Settings == null) {
            CreateSettings(save:true);
        }
        LoadSettings();
    }

    // ------------------------------------------------------------------------
    // settings menu uses to init settings obj
    public void CreateSettings (bool save) {
        SaveDataLoader.CreateSettings(
            GameData.defaultPronounSubj,
            GameData.defaultPronounObj,
            GameData.defaultPronounPos,
            GameData.defaultName,
            save
        );
    }

    // ------------------------------------------------------------------------
    // called every time settings menu is opened
    public void LoadSettings () {
        SaveDataLoader.LoadSettings();
    }

    // ------------------------------------------------------------------------
    public void SaveGame () {
        Assert.IsTrue(
            m_initialized,
            "App not initialized when attempting to save."
        );

        SaveDataLoader.SavePlayerData();
    }

    // ------------------------------------------------------------------------
    public void SaveSettings () {
        SaveDataLoader.SaveSettings();
    }

    // ------------------------------------------------------------------------
    // apply save data to game
    private void PropagateSaveData () {
        // apply all found clues
        foreach(ClueScriptableObject clueObj in GameData.Clues) {
            if(SaveDataLoader.SaveData.FoundClues.Contains(clueObj.ClueID)) {
                clueObj.Unlocked = true;
            }
        }

        // apply all chat progression
        foreach(
            ChatProgressionData chatProgression in
            SaveDataLoader.SaveData.ChatProgressionData
        ) {
            ChatScriptableObject chatObj = GameData.Chats.First(
                c => c.ID == chatProgression.ID
            );
            chatObj.LoadProgression(chatProgression, GameData.Clues);
        }
    }

    // ------------------------------------------------------------------------
    // Methods: Private
    // ------------------------------------------------------------------------
    private void StartFTUE () {
        //TODO
    }

    // ------------------------------------------------------------------------
    private void CloseAllApps() {
        foreach(App app in Apps) {
            if(app.IsOpen) {
                app.Close();
            }
        }
    }

#if DEBUG
    // ------------------------------------------------------------------------
    // Methods: Debug
    // ------------------------------------------------------------------------
    public void DebugToggleClue (string clueName) {
        ClueID id = ClueID.NoClue;
        foreach(ClueScriptableObject clue in GameData.Clues) {
            if(clue.ClueID.ToString().Equals(clueName)) {
                id = clue.ClueID;
                break;
            }
        }

        if(id != ClueID.NoClue) {
            ClueScriptableObject clue = GameData.GetClue(id);
            clue.Unlocked = !clue.Unlocked;
            Debug.Log("clue: " + clue.ClueID + "; state: " + clue.Unlocked);
        }
    }
#endif
}
