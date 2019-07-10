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
    private DataLoader DataLoader;
    public ChatRunner ChatRunner;
    public NotificationManager NotificationManager;
    public NotesApp NotesApp;
    public ChatApp ChatApp;
    public GameObject ReturnButton;
    public List<App> Apps;
    public App HomeApp;

    private List<Chat> m_allChats;
    private App m_activeApp;

    private List<ForumPost> m_allForumPosts;
    private ForumPost m_activeForumPost;

    private List<GramPost> m_allGramPosts;

    private List<MusicUser> m_allMusicUsers;

    private List<Clue> m_clues;
    private List<Photo> m_photos;
    private List<GramUser> m_gramUsers;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public List<Chat> ActiveChats {
        get {
            // only add chats that are available
            List<Chat> activeChats = new List<Chat>();
            foreach(Chat c in m_allChats) {
                //Debug.Log("checking if clue met for chat " + c.Friend + "; clue needed: " + c.ClueNeeded.ToString());
                if(ClueRequirementMet(c.ClueNeeded)) {
                    activeChats.Add(c);
                }
            }
            return activeChats;
        }
    }

    public List<ForumPost> ActiveForumPosts {
        get {
            // only add posts that are available
            List<ForumPost> activePosts = new List<ForumPost>();
            foreach(ForumPost p in m_allForumPosts) {
                if(ClueRequirementMet(p.ClueNeeded)) {
                    activePosts.Add(p);
                }
            }
            return activePosts;
        }
    }

    public List<GramPost> ActiveGramPosts {
        get {
            List<GramPost> activePosts = new List<GramPost>();
            foreach(GramPost p in m_allGramPosts) {
                if(ClueRequirementMet(p.ClueNeeded)) {
                    activePosts.Add(p);
                }
            }
            return activePosts;
        }
    }

    public List<MusicUser> ActiveMusicUsers {
        get {
            List<MusicUser> activeUsers = new List<MusicUser>();
            foreach(MusicUser user in m_allMusicUsers) {
                if(ClueRequirementMet(user.ClueNeeded) && !user.IsPlayer) {
                    activeUsers.Add(user);
                }
            }
            return activeUsers;
        }
    }

    public List<Clue> UnlockedClues {
        get {
            // only add unlocked clues
            List<Clue> clues = new List<Clue>();
            foreach(Clue clue in m_clues) {
                if(ClueRequirementMet(clue.ClueID)) {
                    clues.Add(clue);
                }
            }
            return clues;
        }
    }

    public List<Photo> FoundPhotos {
        get {
            // only add found photos
            List<Photo> photos = new List<Photo>();
            foreach(Photo photo in m_photos) {
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
        Screen.SetResolution(480, 848, false);

        m_allChats = DataLoader.LoadChats();
        m_allForumPosts = DataLoader.LoadForumPosts();

        m_clues = DataLoader.LoadClues();
        m_photos = DataLoader.LoadPhotos();

        m_gramUsers = DataLoader.LoadGramUsers();
        m_allGramPosts = DataLoader.LoadGramPosts();

        m_allMusicUsers = DataLoader.LoadMusicUsers();

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
    public Clue GetClue (ClueID id) {
        Clue clue = m_clues.First(c => c.ClueID == id);
        return clue;
    }

    // ------------------------------------------------------------------------
    public bool ClueRequirementMet (ClueID id) {
        Clue clue = GetClue(id);
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

        Clue clue = GetClue(id);
        // if this is a phone number, send a new contact notif
        if(clue.PhoneNumberGiven != Friend.NoFriend) {
            NotificationManager.NewContactNotif(clue.PhoneNumberGiven);
        } else {
            // otherwise, send generic clue notif
            NotificationManager.FoundClueNotif(id); // really should send an event but meh
        }

        // if it unlocks a ruddit post, send a ruddit notif
        foreach(ForumPost post in m_allForumPosts) {
            if(post.ClueNeeded == id) {
                NotificationManager.ForumPostNotif(post);
            }
        }

        // if a photo needed this clue, mark it found
        foreach(Photo photo in m_photos) {
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
    public Sprite GetPhotoSprite (int index) {
        return DataLoader.PhotoAssets[index];
    }
    
    // ------------------------------------------------------------------------
    public Photo GetPhoto (PhotoID id) {
        return m_photos.FirstOrDefault(p => p.PhotoID == id);
    }

    // ------------------------------------------------------------------------
    public Photo GetPhoto (ClueID id) {
        return m_photos.FirstOrDefault(p => p.ClueID == id);
    }

    // ------------------------------------------------------------------------
    public GramUser GetGramUser (GramUserId id) {
        return m_gramUsers.FirstOrDefault(u => u.UserId == id);
    }

    // ------------------------------------------------------------------------
    public MusicUser GetMusicUser (MusicUserId id) {
        return m_allMusicUsers.FirstOrDefault(u => u.UserID == id);
    }

    // ------------------------------------------------------------------------
    public Sprite GetIcon (int index) {
        return DataLoader.UserIconAssets[index];
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
        foreach(Clue clue in m_clues) {
            if(clue.ClueID.ToString().Equals(clueName)) {
                id = clue.ClueID;
                break;
            }
        }

        if(id != ClueID.NoClue) {
            Clue clue = GetClue(id);
            clue.Unlocked = !clue.Unlocked;
            Debug.Log("clue: " + clue.ClueID + "; state: " + clue.Unlocked);
        }
    }
#endif
}
