using UnityEngine;

[CreateAssetMenu(fileName = "GramUserData", menuName = "IcedCoffee/GramUserScriptableObject", order = 1)]
public class GramUserScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public string Username;
    public Friend UserId;
    public int Icon;
}