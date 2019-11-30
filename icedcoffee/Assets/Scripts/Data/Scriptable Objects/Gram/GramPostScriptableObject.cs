using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class GramPostProgressionData {
    public int ID;
    public bool Liked;
    public int Likes;
    public List<int> PostedComments;

    // only need to save comment & photo if this is ugc
    public bool IsPlayerPost;
    public string Description;
    public PhotoID PhotoID;

    // ------------------------------------------------------------------------
    // use for regular posts
    public GramPostProgressionData (
        int id,
        int likes,
        GramCommentScriptableObject[] comments
    ) {
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
        PhotoID photoID
    ) {
        ID = id;
        Description = description;
        PhotoID = photoID;
        IsPlayerPost = true;

        Liked = false;
        Likes = 0;
        PostedComments = new List<int>();
    }
}

[CreateAssetMenu(fileName = "GramPostData", menuName = "IcedCoffee/GramPostScriptableObject", order = 1)]
public class GramPostScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend UserId;
    public ClueID ClueGiven;
    public ClueID ClueNeeded;
    public string Description;
    public int StartLikes;
    public PhotoID PostImage;
    [SerializeField]
    private GramCommentScriptableObject[] StartComments;
    [SerializeField]
    private List<GramCommentScriptableObject> AllComments;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // auto-generated ID
    [SerializeField]
    private int m_id;
    public int ID {get{return m_id;}}

    private GramPostProgressionData m_progressionData;
    public GramPostProgressionData ProgressionData {
        get{return m_progressionData;}
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

#if DEBUG
    public string DebugName { get {
        string name = UserId.ToString() + ": ";
        if(!string.IsNullOrEmpty(Description)) {
            int lastIndex = Description.Length < 10 ? Description.Length : 10;
            name += Description.Substring(0, lastIndex) + "...";
        }
        return name;
    }}
#endif

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetupPlayerPost (string description, PhotoID photo) {
        Description = description;
        PostImage = photo;
        
        UserId = Friend.You;
        ClueGiven = ClueID.NoClue;
        ClueNeeded = ClueID.NoClue;
        StartLikes = 0;

        StartComments = new GramCommentScriptableObject[0];
        AllComments = new List<GramCommentScriptableObject>();

        m_id = (int)UnityEngine.Random.Range(1, 100000);

        m_progressionData = new GramPostProgressionData(
            m_id, 
            Description,
            PostImage
        );
    }

    // ------------------------------------------------------------------------
    public void ClearProgression () {
        m_progressionData = new GramPostProgressionData(
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
        if(m_progressionData.IsPlayerPost) {
            Description = m_progressionData.Description;
            PostImage = m_progressionData.PhotoID;
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
}