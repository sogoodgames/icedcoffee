using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GramPostUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    // prefabs
    public GameObject CommentPrefab;

    // content references
    public Image TitleProfileImage;
    public Text TitleUsernameText;
    public Button ProfileButton;

    public Image PostImage;
    public Text DescriptionText;
    public RectTransform CommentsParent;

    public Text LikesText;
    public Button LikeButton;

    public Text TimePostedText;
    
    // data refs
    private GramApp GramApp;
    private GramPostScriptableObject postSO;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetPostContent (GramPostScriptableObject post, GramApp app) {
        postSO = post;
        PhoneOS os = app.PhoneOS;
        GramApp = app;

        GramUserScriptableObject user = os.GameData.GetGramUser(postSO.UserId);
        if(user == null) {
            Assert.IsNotNull(user, "GramUser " + postSO.UserId + " not found.");
            return;
        }

        // set post photo content
        PhotoScriptableObject photoSO = postSO.PostImage; 
        if(photoSO != null) {
            PostImage.sprite = photoSO.Image;
        } else {
            Debug.LogError("Gram post image missing.");
        }

        // set user icons
        Sprite userIcon = user.Icon;
        TitleProfileImage.sprite = userIcon;

        // set text
        TitleUsernameText.text = user.Username;
        LikesText.text = postSO.Likes + " likes";
        DescriptionText.text = postSO.Description;

        TimePostedText.text = DialogueProcesser.FormatDateTime(
            postSO.TimePosted
        );

        // set comments
        foreach(GramCommentScriptableObject comment in postSO.Comments) {
            GameObject commentObj = Instantiate (
                CommentPrefab,
                CommentsParent
            );

            GramCommentUI commentUI = commentObj.GetComponent<GramCommentUI>();
            commentUI.SetContent(comment, os);
        }

        // enable/ disable like button
        LikeButton.interactable = !post.Liked;
    }

    // ------------------------------------------------------------------------
    public void Like () {
        if(postSO == null) {
            Assert.IsNotNull(postSO, "Post scriptable object null.");
            return;
        }

        if(GramApp == null) {
            Assert.IsNotNull(GramApp, "GramApp null.");
            return;
        }

        postSO.Like();
        LikesText.text = postSO.Likes + " likes";
        LikeButton.interactable = false;

        GramApp.Save();
    }

    // ------------------------------------------------------------------------
    public void ToggleOpenComments () {
        CommentsParent.gameObject.SetActive(
            !CommentsParent.gameObject.activeInHierarchy
        );
        LayoutRebuilder.ForceRebuildLayoutImmediate(CommentsParent);
    }
}
