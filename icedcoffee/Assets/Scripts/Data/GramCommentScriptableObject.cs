using UnityEngine;

[CreateAssetMenu(fileName = "GramCommentData", menuName = "IcedCoffee/GramCommentScriptableObject", order = 1)]
public class GramCommentScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public Friend UserId;
    public string Comment;
}