using System.Collections.Generic;
using UnityEngine;
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
    
    // data refs
    private GramPostScriptableObject postSO;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetPostContent (GramPostScriptableObject post, PhoneOS os) {
        postSO = post;

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
        postSO.Likes++;
        postSO.Liked = true;
        LikesText.text = postSO.Likes + " likes";
        LikeButton.interactable = false;
    }

    // ------------------------------------------------------------------------
    public void ToggleOpenComments () {
        CommentsParent.gameObject.SetActive(
            !CommentsParent.gameObject.activeInHierarchy
        );
        LayoutRebuilder.ForceRebuildLayoutImmediate(CommentsParent);
    }
}
