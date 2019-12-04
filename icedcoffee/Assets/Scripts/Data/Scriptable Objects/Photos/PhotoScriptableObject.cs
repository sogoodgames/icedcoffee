using UnityEngine;

[CreateAssetMenu(fileName = "PhotoData", menuName = "IcedCoffee/ScriptableObjects/Photo", order = 1)]
public class PhotoScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PhotoID PhotoID;
    public ClueID ClueID;
    public Sprite Image;
    public float Width;
    public float Height;
    public string Description;

    [HideInInspector]
    public bool Found;
}