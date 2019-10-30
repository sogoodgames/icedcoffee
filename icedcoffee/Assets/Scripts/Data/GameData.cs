using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class GameData : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string defaultPronounSubj = "they";
    public string defaultPronounObj = "them";
    public string defaultPronounPos = "their";
    public string defaultName = "Alex";

    [SerializeField]
    private List<MusicUserScriptableObject> _musicUsers;
    [SerializeField]
    private List<ChatScriptableObject> _chats;
    [SerializeField]
    private List<GramPostScriptableObject> _gramPosts;
    [SerializeField]
    private List<GramUserScriptableObject> _gramUsers;
    [SerializeField]
    private List<ForumPostScriptableObject> _forumPosts;
    [SerializeField]
    private List<ForumUserScriptableObject> _forumUsers;
    [SerializeField]
    private List<ClueScriptableObject> _clues;
    [SerializeField]
    private List<PhotoScriptableObject> _photos;
    [SerializeField]
    private List<FriendScriptableObject> _friends;
    [SerializeField]
    private List<Sprite> _photoAssets;
    [SerializeField]
    private Sprite _groupChatIcon;

    private Dictionary<Friend, MusicUserScriptableObject> _musicUsersInstanced;
    private Dictionary<int, ChatScriptableObject> _chatsInstanced;
    private Dictionary<Friend, GramPostScriptableObject> _gramPostsInstanced;
    private Dictionary<Friend, GramUserScriptableObject> _gramUsersInstanced;
    private Dictionary<Friend, ForumPostScriptableObject> _forumPostsInstanced;
    private Dictionary<Friend, ForumUserScriptableObject> _forumUsersInstanced;
    private Dictionary<ClueID, ClueScriptableObject> _cluesInstanced;
    private Dictionary<ClueID, PhotoScriptableObject> _photosInstancedByClue;
    private Dictionary<PhotoID, PhotoScriptableObject> _photosInstancedById;
    private Dictionary<Friend, FriendScriptableObject> _friendsInstanced;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public List<MusicUserScriptableObject> MusicUsers {
        get {
            Assert.IsNotNull(_musicUsersInstanced, "Music users list null.");
            return new List<MusicUserScriptableObject>(
                _musicUsersInstanced.Values
            );
        }
    }

    public List<ChatScriptableObject> Chats {
        get{
            if(_chatsInstanced == null) Init();
            return new List<ChatScriptableObject>(_chatsInstanced.Values);
        }
    }

    public List<GramPostScriptableObject> GramPosts {
        get{return new List<GramPostScriptableObject>(_gramPostsInstanced.Values);}
    }

    public List<GramUserScriptableObject> GramUsers {
        get{return new List<GramUserScriptableObject>(_gramUsersInstanced.Values);}
    }

    public List<ForumPostScriptableObject> ForumPosts {
        get{return new List<ForumPostScriptableObject>(_forumPostsInstanced.Values);}
    }

    public List<ForumUserScriptableObject> ForumUsers {
        get{return new List<ForumUserScriptableObject>(_forumUsersInstanced.Values);}
    }

    public List<ClueScriptableObject> Clues {
        get{return new List<ClueScriptableObject>(_cluesInstanced.Values);}
    }

    public List<PhotoScriptableObject> Photos {
        get{return new List<PhotoScriptableObject>(_photosInstancedByClue.Values);}
    }

    public List<FriendScriptableObject> Friends {
        get{return new List<FriendScriptableObject>(_friendsInstanced.Values);}
    }

    public List<Sprite> PhotoAssets {
        get{return _photoAssets;}
    }

    public Sprite GroupChatIcon {
        get{return _groupChatIcon;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Init () {
        _musicUsersInstanced = new Dictionary<Friend, MusicUserScriptableObject>();
        foreach (MusicUserScriptableObject m in _musicUsers) {
            Assert.IsNotNull (
                m,
                "Music user null."
            );
            _musicUsersInstanced.Add(m.FriendID, m);
        }

        _chatsInstanced = new Dictionary<int, ChatScriptableObject>();
        foreach (ChatScriptableObject c in _chats) {
            _chatsInstanced.Add(c.ID, c);
        }

        _gramPostsInstanced = new Dictionary<Friend, GramPostScriptableObject>();
        foreach (GramPostScriptableObject g in _gramPosts) {
            _gramPostsInstanced.Add(g.UserId, g);
        }

        _gramUsersInstanced = new Dictionary<Friend, GramUserScriptableObject>();
        foreach (GramUserScriptableObject g in _gramUsers) {
            _gramUsersInstanced.Add(g.UserId, g);
        }

        _forumPostsInstanced = new Dictionary<Friend, ForumPostScriptableObject>();
        foreach (ForumPostScriptableObject f in _forumPosts) {
            _forumPostsInstanced.Add(f.UserID, f);
        }

        _forumUsersInstanced = new Dictionary<Friend, ForumUserScriptableObject>();
        foreach (ForumUserScriptableObject f in _forumUsers) {
            _forumUsersInstanced.Add(f.UserID, f);
        }

        _cluesInstanced = new Dictionary<ClueID, ClueScriptableObject>();
        foreach (ClueScriptableObject c in _clues) {
            _cluesInstanced.Add(c.ClueID, c);
        }

        _photosInstancedByClue = new Dictionary<ClueID, PhotoScriptableObject>();
        foreach (PhotoScriptableObject p in _photos) {
            _photosInstancedByClue.Add(p.ClueID, p);
        }

        _photosInstancedById = new Dictionary<PhotoID, PhotoScriptableObject>();
        foreach (PhotoScriptableObject p in _photos) {
            _photosInstancedById.Add(p.PhotoID, p);
        }

        _friendsInstanced = new Dictionary<Friend, FriendScriptableObject>();
        foreach (FriendScriptableObject f in _friends) {
            _friendsInstanced.Add(f.Friend, f);
        }
    }

    // ------------------------------------------------------------------------
    public FriendScriptableObject GetFriend (Friend friend) {
        if(_friendsInstanced.ContainsKey(friend)) {
            return _friendsInstanced[friend];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public ClueScriptableObject GetClue (ClueID id) {
        if(_cluesInstanced.ContainsKey(id)) {
            return _cluesInstanced[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public PhotoScriptableObject GetPhoto (ClueID id) {
        if(_photosInstancedByClue.ContainsKey(id)) {
            return _photosInstancedByClue[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public PhotoScriptableObject GetPhoto (PhotoID id) {
        if(_photosInstancedById.ContainsKey(id)) {
            return _photosInstancedById[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public GramUserScriptableObject GetGramUser (Friend id) {
        if(_gramPostsInstanced.ContainsKey(id)) {
            return _gramUsersInstanced[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public ForumUserScriptableObject GetForumUser (Friend id) {
        if(_forumUsersInstanced.ContainsKey(id)) {
            return _forumUsersInstanced[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public MusicUserScriptableObject GetMusicUser (Friend id) {
        if(_musicUsersInstanced.ContainsKey(id)) {
            return _musicUsersInstanced[id];
        }
        return null;
    }

    // ------------------------------------------------------------------------
    public ChatScriptableObject GetChat (int id) {
        if(_chatsInstanced.ContainsKey(id)) {
            return _chatsInstanced[id];
        }
        return null;
    }
}