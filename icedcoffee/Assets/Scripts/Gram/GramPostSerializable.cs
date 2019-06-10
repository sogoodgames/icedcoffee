using System;   // serializable

[Serializable] 
public class GramUserSerializable {
    public string username;
    public GramUserId userId;
    public int icon;
}

[Serializable]
public class GramCommentSerializable {
    public GramUserId userId;
    public string comment;
}

[Serializable]
public class GramPostSerializable {
    public GramUserId userId;
    public ClueID clueGiven;
    public ClueID clueNeeded;
    public string description;
    public PhotoID postImage;
    public GramCommentSerializable[] comments;
}