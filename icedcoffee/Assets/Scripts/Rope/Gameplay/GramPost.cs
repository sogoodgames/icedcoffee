using System; // serializable

public class GramUser {
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private string m_username;
    public string Username {
        get{return m_username;}
    }

    private GramUserId m_userId;
    public GramUserId UserId {
        get{return m_userId;}
    }

    private int m_icon;
    public int Icon {
        get{return m_icon;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public GramUser (GramUserSerializable user) {
        m_username = user.username;
        m_userId = user.userId;
        m_icon = user.icon;
    }
}

public class GramComment {
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private GramUserId m_userId;
    public GramUserId UserId {
        get{return m_userId;}
    }

    private string m_comment;
    public string Comment {
        get{return m_comment;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public GramComment (GramCommentSerializable comment) {
        m_userId = comment.userId;
        m_comment = comment.comment;
    }
}

public class GramPost {
    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private GramUserId m_userId;
    public GramUserId UserId {
        get{return m_userId;}
    }

    private ClueID m_clueNeeded;
    public ClueID ClueNeeded {
        get{return m_clueNeeded;}
    }

    private ClueID m_clueGiven;
    public ClueID ClueGiven {
        get{return m_clueGiven;}
    }

    private string m_description;
    public string Description {
        get{return m_description;}
    }

    private PhotoID m_postImage;
    public PhotoID PostImage {
        get{return m_postImage;}
    }

    private GramComment[] m_comments;
    public GramComment[] Comments {
        get{return m_comments;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public GramPost (GramPostSerializable post) {
        m_userId = post.userId;
        m_clueGiven = post.clueGiven;
        m_clueNeeded = post.clueNeeded;
        m_description = post.description;
        m_postImage = post.postImage;

        m_comments = new GramComment[post.comments.Length];
        for(int i = 0; i < post.comments.Length; i++) {
            m_comments[i] = new GramComment(post.comments[i]);
        }
    }
}