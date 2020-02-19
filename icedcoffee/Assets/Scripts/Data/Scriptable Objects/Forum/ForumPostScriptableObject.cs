using UnityEngine;

[CreateAssetMenu(fileName = "ForumPostData", menuName = "IcedCoffee/ScriptableObjects/ForumPost", order = 1)]
public class ForumPostScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend UserID;
    public string Title; // post title
    public ClueScriptableObject ClueGivenSO; // the clue given (if any)
    public ClueScriptableObject ClueNeededSO;
    public string Body; // text in the post
    public int NumComments; // number of comments
    public int Time; // minutes ago it was posted
    public PhotoID Photo; // post image file (optional)

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public string DebugName {
        get {
            return UserID.ToString() + "- " + Title;
        }
    }

    public ClueID ClueNeeded {
        get{ return ClueNeededSO == null? ClueID.NoClue : ClueNeededSO.ClueID;}
    }

    public ClueID ClueGiven {
        get{ return ClueGivenSO == null? ClueID.NoClue : ClueGivenSO.ClueID;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public bool Empty () {
        return string.IsNullOrEmpty(Body);
    }
}