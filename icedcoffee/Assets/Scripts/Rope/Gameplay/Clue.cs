public class Clue {
    // variables
    public bool Unlocked;
    
    // properties
    private bool m_canSend;
    public bool CanSend {
        get{return m_canSend;}
    }

    private Friend m_phoneNumberGiven;
    public Friend PhoneNumberGiven {
        get{return m_phoneNumberGiven;}
    }

    private ClueID m_clueID;
    public ClueID ClueID {
        get{return m_clueID;}
    }

    private string m_note;
    public string Note {
        get{return m_note;}
    }

    private string m_messageText;
    public string MessageText {
        get{return m_messageText;}
    }

    public bool Invalid {
        get{return string.IsNullOrEmpty(m_note);}
    }

    // methods
    public Clue(ClueSerializable serializedClue) {
        Unlocked = serializedClue.unlocked;
        m_phoneNumberGiven = serializedClue.phoneNumberGiven;
        m_clueID = serializedClue.clueID;
        m_note = serializedClue.note;
        m_canSend = serializedClue.canSend;
        m_messageText = serializedClue.messageText;
    }
}