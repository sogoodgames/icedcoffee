using UnityEngine;

public class ChatSelectionUI : MonoBehaviour
{
    public virtual void Open () {
        gameObject.SetActive(true);
    }

    public virtual void Open (Chat chat) {
        gameObject.SetActive(true);
    }

    public virtual void Close () {
        gameObject.SetActive(false);
    }
}