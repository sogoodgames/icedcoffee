public class Photo {
    public bool Found;

    private PhotoID m_id;
    public PhotoID PhotoID {
        get{return m_id;}
    }

    private ClueID m_clueId;
    public ClueID ClueID {
        get{return m_clueId;}
    }

    private int m_image;
    public int Image {
        get{return m_image;}
    }

    private float m_width;
    public float Width {
        get{return m_width;}
    }

    private float m_height;
    public float Height {
        get{return m_height;}
    }

    private string m_description;
    public string Description {
        get{return m_description;}
    }

    public Photo (PhotoSerializable photo) {
        m_id = photo.photoID;
        m_image = photo.image;
        m_width = photo.width;
        m_height = photo.height;
        m_description = photo.description;
        m_clueId = photo.clueID;
    }
}