using System;   // serializable

[Serializable]
public class PhotoSerializable {
    public PhotoID photoID;
    public ClueID clueID;
    public int image;
    public float width;
    public float height;
    public string description;
}