using UnityEngine;

public class SlideAnimation : MonoBehaviour {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public AnimationCurve AppAnimationCurve;
    public float AppAnimationSpeed = 2000;
    public float AppAnimationTime = 0.5f;
    public float AppStartX = 1080.0f;

    public delegate void SlideAnimationFinishedDelegate();
    public event SlideAnimationFinishedDelegate SlideAnimationFinished;

    protected bool animate;
    private float animTime;
    private int xDir;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Update () {
        if(animate) {
            float animTimeLinear = animTime / AppAnimationTime;
            float animCurve = AppAnimationCurve.Evaluate(animTimeLinear);
            transform.Translate(
                xDir * animCurve * AppAnimationSpeed * Time.deltaTime,
                0, 0
            );
            animTime += Time.deltaTime;

            bool reachedDestination = false;
            if(xDir == 1) {
                reachedDestination = transform.localPosition.x >= AppStartX;
            } else {
                reachedDestination = transform.localPosition.x <= 0.0f;
            }

            if(reachedDestination) {
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z); 
                animate = false;
                if(SlideAnimationFinished != null)
                    SlideAnimationFinished();
            }
        }
    }

    // ------------------------------------------------------------------------
    public void PlaySlideAnimation (int direction) {
        animate = true;
        animTime = 0;
        xDir = direction;

        if(xDir == -1) {
            transform.localPosition = new Vector3(AppStartX, transform.localPosition.y, transform.localPosition.z);
        }
    }
}