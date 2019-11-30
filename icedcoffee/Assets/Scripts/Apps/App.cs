using UnityEngine;

public abstract class App : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PhoneOS PhoneOS;
    public Sprite Icon;
    public bool DoSlideAnimation = true;
    public SlideAnimation SlideAnimator;

    public bool IsOpen {
        get {return gameObject.activeInHierarchy;}
    }

    protected bool m_waitingForClose;

    // ------------------------------------------------------------------------
    // Methods: MonoBehaviour
    // ------------------------------------------------------------------------
    protected virtual void Awake () {
        if(DoSlideAnimation) {
            SlideAnimator.SlideAnimationFinished += HandleSlideAnimationFinished;
        } else {
            m_waitingForClose = false;
        }
    }

    // ------------------------------------------------------------------------
    // Methods: App
    // ------------------------------------------------------------------------
    public virtual void Open () {
        gameObject.SetActive(true);
        if(DoSlideAnimation) {
            m_waitingForClose = false;
            SlideAnimator.PlaySlideAnimation(-1);
        }
    }

    // ------------------------------------------------------------------------
    public virtual void Close () {
        if(DoSlideAnimation) {
            m_waitingForClose = true;
            SlideAnimator.PlaySlideAnimation(1);
        } else {
            HandleSlideAnimationFinished();
        }
    }

    // ------------------------------------------------------------------------
    public virtual void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            gameObject.SetActive(false);
            m_waitingForClose = false;
        }
    }

    // ------------------------------------------------------------------------
    public virtual void Return () {
        PhoneOS.GoHome();
    }

    // ------------------------------------------------------------------------
    public void Save () {
        PhoneOS.SaveGame();
    }
}
