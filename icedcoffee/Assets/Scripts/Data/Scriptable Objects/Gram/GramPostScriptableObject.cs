using UnityEngine;

[CreateAssetMenu(fileName = "GramPostData", menuName = "IcedCoffee/GramPostScriptableObject", order = 1)]
public class GramPostScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public Friend UserId;
    public ClueID ClueGiven;
    public ClueID ClueNeeded;
    public string Description;
    public PhotoID PostImage;
    public GramCommentScriptableObject[] Comments;

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
}