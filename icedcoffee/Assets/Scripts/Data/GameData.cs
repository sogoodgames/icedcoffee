using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
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
    private List<ClueScriptableObject> _clues;
    [SerializeField]
    private List<PhotoScriptableObject> _photos;
    [SerializeField]
    private List<Sprite> _userIconAssets;
    [SerializeField]
    private List<Sprite> _photoAssets;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public List<MusicUserScriptableObject> MusicUsers {
        get {return _musicUsers;}
    }

    public List<ChatScriptableObject> Chats {
        get{return _chats;}
    }

    public List<GramPostScriptableObject> GramPosts {
        get{return _gramPosts;}
    }

    public List<GramUserScriptableObject> GramUsers {
        get{return _gramUsers;}
    }

    public List<ForumPostScriptableObject> ForumPosts {
        get{return _forumPosts;}
    }

    public List<ClueScriptableObject> Clues {
        get{return _clues;}
    }

    public List<PhotoScriptableObject> Photos {
        get{return _photos;}
    }

    public List<Sprite> UserIconAssets {
        get{return _userIconAssets;}
    }

    public List<Sprite> PhotoAssets {
        get{return _photoAssets;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------

}