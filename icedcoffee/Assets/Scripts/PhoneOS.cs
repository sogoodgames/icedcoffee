#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
#define DEBUG
#endif

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class PhoneOS : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public bool RunFTUE = true;

    [SerializeField]
    private GameData GameData;
    public ChatRunner ChatRunner;
    public NotificationManager NotificationManager;
    public NotesApp NotesApp;
    public ChatApp ChatApp;
    public GameObject ReturnButton;
    public List<App> Apps;
    public App HomeApp;

    private SaveDataLoader SaveDataLoader;
    private App m_activeApp;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
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
                if(ClueRequirementMet(user.ClueNeeded) && !user.IsPlayer) {
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
    void Awake () {
        // load save data
        SaveDataLoader = new SaveDataLoader();
        Debug.Log("Loaded save data with start time: " + SaveDataLoader.SaveData.GameStartTime.ToString());

        // set screen resolution
        Screen.SetResolution(480, 848, false);

        // subscribe to game events
        ChatRunner.FoundClue += FoundClue;
    }

    // ------------------------------------------------------------------------
    void OnEnable () {
        if(RunFTUE) {
            StartFTUE();
        } else {
            m_activeApp = HomeApp;
        }
    }
    
    // ------------------------------------------------------------------------
    // Methods: Phone navigation
    // ------------------------------------------------------------------------
    public void OpenApp (App app) {
        OpenApp(app, false);
    }

    // ------------------------------------------------------------------------
    public void OpenApp (App app, bool force) {
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
    public ClueScriptableObject GetClue (ClueID id) {
        ClueScriptableObject clue = GameData.Clues.First(c => c.ClueID == id);
        return clue;
    }

    // ------------------------------------------------------------------------
    public bool ClueRequirementMet (ClueID id) {
        ClueScriptableObject clue = GetClue(id);
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

        ClueScriptableObject clue = GetClue(id);
        // if this is a phone number, send a new contact notif
        if(clue.PhoneNumberGiven != Friend.NoFriend) {
            NotificationManager.NewContactNotif(clue.PhoneNumberGiven);
        } else {
            // otherwise, send generic clue notif
            NotificationManager.FoundClueNotif(id); // really should send an event but meh
        }

        // if it unlocks a ruddit post, send a ruddit notif
        foreach(ForumPostScriptableObject post in GameData.ForumPosts) {
            if(post.ClueNeeded == id) {
                NotificationManager.ForumPostNotif(post);
            }
        }

        // if a photo needed this clue, mark it found
        foreach(PhotoScriptableObject photo in GameData.Photos) {
            //Debug.Log("clue id: " + photo.ClueID + "; looking for: " + id);
            if(photo.ClueID == id) {
                photo.Found = true;
            }
        }

        clue.Unlocked = true;
    }

    // ------------------------------------------------------------------------
    // Methods: Data
    // ------------------------------------------------------------------------
    public PhotoScriptableObject GetPhoto (PhotoID id) {
        return GameData.Photos.FirstOrDefault(p => p.PhotoID == id);
    }

    // ------------------------------------------------------------------------
    public PhotoScriptableObject GetPhoto (ClueID id) {
        return GameData.Photos.FirstOrDefault(p => p.ClueID == id);
    }

    // ------------------------------------------------------------------------
    public GramUserScriptableObject GetGramUser (Friend id) {
        return GameData.GramUsers.FirstOrDefault(u => u.UserId == id);
    }

    // ------------------------------------------------------------------------
    public MusicUserScriptableObject GetMusicUser (Friend id) {
        return GameData.MusicUsers.FirstOrDefault(u => u.FriendID == id);
    }

    // ------------------------------------------------------------------------
    public ForumUserScriptableObject GetForumUser (Friend id) {
        return GameData.ForumUsers.FirstOrDefault(u => u.UserID == id);
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
            ClueScriptableObject clue = GetClue(id);
            clue.Unlocked = !clue.Unlocked;
            Debug.Log("clue: " + clue.ClueID + "; state: " + clue.Unlocked);
        }
    }
#endif
}
