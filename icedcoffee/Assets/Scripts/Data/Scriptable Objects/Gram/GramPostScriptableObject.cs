using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public struct GramPostProgressionData {
    public int ID;
    public bool Liked;
    public int Likes;
    public List<int> PostedComments;

    public GramPostProgressionData (int id, int likes) {
        ID = id;
        Likes = likes;
        Liked = false;
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
    public GramCommentScriptableObject[] Comments;

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
    public void ClearProgression () {
        m_progressionData = new GramPostProgressionData(m_id, StartLikes);
    }

    // ------------------------------------------------------------------------
    public void LoadProgression (GramPostProgressionData progressionData) {
        m_progressionData = progressionData;
    }

    // ------------------------------------------------------------------------
    public void Like () {
        m_progressionData.Likes++;
        m_progressionData.Liked = true;
    }
}