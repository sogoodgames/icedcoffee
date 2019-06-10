public class Clue {
    // variables
    public bool Unlocked;
    
    // properties
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

    public bool Invalid {
        get{return string.IsNullOrEmpty(m_note);}
    }

    // methods
    public Clue(ClueSerializable serializedClue) {
        Unlocked = serializedClue.unlocked;
        m_phoneNumberGiven = serializedClue.phoneNumberGiven;
        m_clueID = serializedClue.clueID;
        m_note = serializedClue.note;
    }
}