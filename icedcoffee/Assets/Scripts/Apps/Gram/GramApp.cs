using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GramApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Transform GramPostParent;
    public GameObject GramPostPrefab;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open () {
        base.Open();
        PopulatePosts();
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            foreach(Transform child in GramPostParent.transform) {
                Destroy(child.gameObject);
            }
        }
        base.HandleSlideAnimationFinished();
    }

    // ------------------------------------------------------------------------
    private void PopulatePosts () {
        foreach(GramPostScriptableObject post in PhoneOS.ActiveGramPosts) {
            GameObject postObj = Instantiate(
                GramPostPrefab,
                GramPostParent
            );

            GramPostUI postUI = postObj.GetComponent<GramPostUI>();
            postUI.SetPostContent(post, PhoneOS);
        }
    }
}