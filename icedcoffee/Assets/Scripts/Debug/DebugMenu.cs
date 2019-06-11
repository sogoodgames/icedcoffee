using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    // PUBLIC REFERENCES TO EVERYTHING
    // CAUSE THIS IS DEBUG AND IDGAF
    public PhoneOS PhoneOS;
    public ChatRunner ChatRunner;

    private float m_cachedMessageDelay;

    void OnEnable () {
        m_cachedMessageDelay = ChatRunner.MaxTimeBetweenMessages;
    }

    public void ToggleOpen () {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ToggleInstantChat () {
        if(ChatRunner.MaxTimeBetweenMessages < 0.001) {
            ChatRunner.MaxTimeBetweenMessages = m_cachedMessageDelay;
        } else {
            ChatRunner.MaxTimeBetweenMessages = 0;
        }
    }
}
