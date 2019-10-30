using UnityEngine;

[CreateAssetMenu(fileName = "GramCommentData", menuName = "IcedCoffee/GramCommentScriptableObject", order = 1)]
public class GramCommentScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public Friend UserId;
    public string Comment;

#if DEBUG
    public string DebugName {
        get {
            string name = UserId.ToString();
            if(!string.IsNullOrEmpty(Comment)) {
                int lastIndex = Comment.Length < 10 ? Comment.Length : 10; 
                name += "- " + Comment.Substring(0, lastIndex) + "...";
            }
            return name;
        }
    }
#endif
}