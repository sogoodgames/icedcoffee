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
        if(user == null) return;
        PhotoScriptableObject postPhoto = os.GameData.GetPhoto(postSO.PostImage);
        if(postPhoto == null) return;

        // set post photo content
        Sprite postSprite = postPhoto.Image;
        PostImage.sprite = postSprite;

        // set user icons
        Sprite userIcon = user.Icon;
        TitleProfileImage.sprite = userIcon;

        // set text
        TitleUsernameText.text = user.Username;
        LikesText.text = postSO.Likes + " likes";
        DescriptionText.text = postSO.Description;

        TimePostedText.text = postSO.TimePosted.ToString();

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
