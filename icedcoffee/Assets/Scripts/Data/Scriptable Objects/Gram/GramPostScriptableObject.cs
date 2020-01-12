using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

public enum GramPostType {
    OldPost = 0, // posts which are authored to appear when the game starts
    NewNpcPost = 1, // posts which appear during the gameplay
    PlayerPost = 2 // posts which the player writes
}

[Serializable]
public class GramPostProgressionData {
    public int ID;
    public bool Liked;
    public int Likes;
    public List<int> PostedComments;
    public GramPostType PostType;
    public long PostTimeTicks;

    // only need to save content if this is ugc
    // (because ugc won't have a matching object in Resources)
    public string Description;
    public PhotoID PhotoID;

    // ------------------------------------------------------------------------
    // use for regular posts
    public GramPostProgressionData (
        GramPostType postType,
        int id,
        int likes,
        GramCommentScriptableObject[] comments
    ) {
        PostType = postType;
        ID = id;
        Likes = likes;
        Liked = false;
        
        PostedComments = new List<int>();
        foreach(GramCommentScriptableObject commentObj in comments) {
            PostedComments.Add(commentObj.ID);
        }
    }

    // ------------------------------------------------------------------------
    // use for user posts
    public GramPostProgressionData (
        int id, 
        string description,
        PhotoID photoID,
        long postTimeTicks
    ) {
        PostType = GramPostType.PlayerPost;
        ID = id;
        Description = description;
        PhotoID = photoID;
        PostTimeTicks = postTimeTicks;

        Liked = false;
        Likes = 0;
        PostedComments = new List<int>();
    }
}

[CreateAssetMenu(fileName = "GramPostData", menuName = "IcedCoffee/ScriptableObjects/GramPost", order = 1)]
public class GramPostScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend UserId;
    public GramPostType PostType;
    public ClueID ClueGiven;
    public ClueID ClueNeeded;
    public string Description;
    public int StartLikes;
    public PhotoID PostImage;
    [SerializeField]
    private GramCommentScriptableObject[] StartComments;
    [SerializeField]
    private List<GramCommentScriptableObject> AllComments;

    // once we instantiate this object, save the post time in the progression
    // as datetime ticks
    // # days relative to start of game when post was made
    [SerializeField]
    private int PostTimeDays;
    // hour the post was posted
    [SerializeField]
    private int PostTimeHour;
    // minute the post was posted
    [SerializeField]
    private int PostTimeMinute;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // auto-generated ID
    [SerializeField]
    private int m_id;
    public int ID {get{return m_id;}}

    private GramPostProgressionData m_progressionData;
    public GramPostProgressionData ProgressionData {
        get{ return m_progressionData; }
    }

    public DateTime TimePosted { 
        get { return new DateTime(
                m_progressionData.PostTimeTicks,
                DateTimeKind.Local
            ); 
        }
    }

    // TODO: if we find performance is an issue, maybe cache comments 
    // like how messages in a chat are cached-
    // however, i doubt there will be more than ~10 comments ever on a gram post
    public List<GramCommentScriptableObject> Comments {
        get {
            List<GramCommentScriptableObject> comments =
                new List<GramCommentScriptableObject>();
            foreach(int id in m_progressionData.PostedComments) {
                GramCommentScriptableObject commentObj = 
                    AllComments.FirstOrDefault(c => c.ID == id);
                if(commentObj != null) {
                    comments.Add(commentObj);
                } else {
                    Assert.IsNotNull(
                        commentObj,
                        "Did not find comment object that matches ID " + id
                    );
                }
            }
            return comments;
        }
    }

    public bool Liked {
        get{ return m_progressionData.Liked; }
    }

    public int Likes {
        get { return m_progressionData.Likes; }
    }

    public string DebugName { get {
        string name = UserId.ToString() + ": ";
        if(!string.IsNullOrEmpty(Description)) {
            int lastIndex = Description.Length < 10 ? Description.Length : 10;
            name += Description.Substring(0, lastIndex) + "...";
        }
        return name;
    }}

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // convert the age of the post from the content settings
    // into an actual date time
    public void InitOldPost (DateTime gameStartTime) {
        if(PostType != GramPostType.OldPost) {
            Debug.LogError("Trying to init an old post, but SO isn't marked as such.");
            return;
        }

        DateTime dateTime = new DateTime(
            year:gameStartTime.Year,
            month:gameStartTime.Month,
            day:gameStartTime.Day - PostTimeDays,
            hour:PostTimeHour,
            minute:PostTimeMinute,
            second:(int)UnityEngine.Random.Range(0,59)
        );
        m_progressionData.PostTimeTicks = dateTime.Ticks;
    }

    // ------------------------------------------------------------------------
    // initialization for when you actually post it
    public void Post (DateTime actualPostTime) {
        if(PostType != GramPostType.NewNpcPost) {
            Debug.LogError("Trying to post a new NPC post, but SO isn't marked as such.");
            return;
        }

        m_progressionData.PostTimeTicks = actualPostTime.Ticks;
    }

    // ------------------------------------------------------------------------
    public void CreatePlayerPost (
        string description,
        PhotoID photo,
        long postTimeTicks
    ) {
        Description = description;
        PostImage = photo;

        StartComments = new GramCommentScriptableObject[0];
        AllComments = new List<GramCommentScriptableObject>();

        m_id = (int)UnityEngine.Random.Range(1, 100000);

        m_progressionData = new GramPostProgressionData(
            m_id, 
            Description,
            PostImage,
            postTimeTicks
        );

        SetupDefaultPlayerData();
    }

    // ------------------------------------------------------------------------
    public void ClearProgression () {
        m_progressionData = new GramPostProgressionData(
            PostType,
            m_id,
            StartLikes,
            StartComments
        );
    }

    // ------------------------------------------------------------------------
    public void LoadProgression (GramPostProgressionData progressionData) {
        m_progressionData = progressionData; 

        // load photo and caption only if this is a player-created post
        // (because there is no matching post in the game data)
        if(m_progressionData.PostType == GramPostType.PlayerPost) {
            Description = m_progressionData.Description;
            PostImage = m_progressionData.PhotoID;

            SetupDefaultPlayerData();
        }
    }

    // ------------------------------------------------------------------------
    public void Like () {
        m_progressionData.Likes++;
        m_progressionData.Liked = true;
    }

    // ------------------------------------------------------------------------
    public void RecordCommentInProgression (GramCommentScriptableObject comment) {
        m_progressionData.PostedComments.Add(comment.ID);
    }

    // ------------------------------------------------------------------------
    private void SetupDefaultPlayerData () {
        PostType = GramPostType.PlayerPost;
        UserId = Friend.You;
        ClueGiven = ClueID.NoClue;
        ClueNeeded = ClueID.NoClue;
        StartLikes = 0;
    }
}