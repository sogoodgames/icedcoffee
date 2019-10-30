using UnityEngine;

[CreateAssetMenu(fileName = "MusicUserData", menuName = "IcedCoffee/MusicUserScriptableObject", order = 1)]
public class MusicUserScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string Username;
    public string PlaylistName;
    public int NumFollowers;
    public Friend FriendID;
    public ClueID ClueNeeded;
    public ClueID ClueGiven;
    public SongScriptableObject[] Playlist;
}