using System;   // serializable

[Serializable]
public class ForumPostSerializable {
    public string username; // poster username
    public string title; // post title
    public ClueID clueGiven; // the clue given (if any)
    public ClueID clueNeeded;
    public string body; // text in the post
    public int numComments; // number of comments
    public int time; // minutes ago it was posted
    public int icon; // icon file
    public PhotoID photo; // post image file (optional)
}