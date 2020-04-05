using UnityEngine;

[CreateAssetMenu(fileName = "MusicUserData", menuName = "IcedCoffee/ScriptableObjects/MusicUser", order = 1)]
public class MusicUserScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string Username;
    public string PlaylistName;
    public int NumFollowers;
    public Friend FriendID;
    public ClueScriptableObject ClueNeededSO;
    public ClueScriptableObject ClueGivenSO;
    public SongScriptableObject[] Playlist;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public ClueID ClueNeeded {
        get{ return ClueNeededSO == null? ClueID.NoClue : ClueNeededSO.ClueID;}
    }

    public ClueID ClueGiven {
        get{ return ClueGivenSO == null? ClueID.NoClue : ClueGivenSO.ClueID;}
    }
}