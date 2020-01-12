using System;

using UnityEngine;

[CreateAssetMenu(fileName = "GramCommentData", menuName = "IcedCoffee/ScriptableObjects/GramComment", order = 1)]
public class GramCommentScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend UserId;
    public string Comment;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    // auto-generated ID
    [SerializeField]
    private int m_id;
    public int ID {get{return m_id;}}

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
}