using UnityEngine;

[CreateAssetMenu(fileName = "GramUserData", menuName = "IcedCoffee/ScriptableObjects/GramUser", order = 1)]
public class GramUserScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public string Username;
    public int NumFollowers;
    public int NumFollowing;
    public string Description;
    public Friend UserId;
    public Sprite Icon;
}