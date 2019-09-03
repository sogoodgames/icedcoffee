using UnityEngine;

[CreateAssetMenu(fileName = "ForumPostData", menuName = "IcedCoffee/ForumPostScriptableObject", order = 1)]
public class ForumPostScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend UserID;
    public string Title; // post title
    public ClueID ClueGiven; // the clue given (if any)
    public ClueID ClueNeeded;
    public string Body; // text in the post
    public int NumComments; // number of comments
    public int Time; // minutes ago it was posted
    public PhotoID Photo; // post image file (optional)

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public bool Empty () {
        return string.IsNullOrEmpty(Body);
    }
}