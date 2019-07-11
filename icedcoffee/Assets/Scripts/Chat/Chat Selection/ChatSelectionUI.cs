using UnityEngine;

public class ChatSelectionUI : MonoBehaviour
{
    public bool IsOpen {
        get{ return gameObject.activeInHierarchy; }
    }

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