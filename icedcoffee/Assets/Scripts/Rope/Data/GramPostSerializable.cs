using System;   // serializable

[Serializable] 
public class GramUserSerializable {
    public string username;
    public Friend userId;
    public int icon;
}

[Serializable]
public class GramCommentSerializable {
    public Friend userId;
    public string comment;
}

[Serializable]
public class GramPostSerializable {
    public Friend userId;
    public ClueID clueGiven;
    public ClueID clueNeeded;
    public string description;
    public PhotoID postImage;
    public GramCommentSerializable[] comments;
}