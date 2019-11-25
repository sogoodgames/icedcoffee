using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "IcedCoffee/SongScriptableObject", order = 1)]
public class SongScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string Title;
    public string Artist;
    public string Album;
    public AudioClip Song;
}