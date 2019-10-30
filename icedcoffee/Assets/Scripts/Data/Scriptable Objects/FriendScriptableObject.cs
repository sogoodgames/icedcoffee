using UnityEngine;

[CreateAssetMenu(fileName = "FriendData", menuName = "IcedCoffee/FriendScriptableObject", order = 1)]
public class FriendScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Friend Friend;
    public string Name;
    public ClueID ContactClue;
    public Sprite Icon;
}
