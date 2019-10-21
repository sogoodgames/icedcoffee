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
    public Image PostImage;
    public Text PostUsernameText;
    public Text DescriptionText;
    public RectTransform CommentsParent;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetPostContent (GramPostScriptableObject post, PhoneOS os) {
        GramUserScriptableObject user = os.GameData.GetGramUser(post.UserId);
        if(user == null) return;
        PhotoScriptableObject postPhoto = os.GameData.GetPhoto(post.PostImage);
        if(postPhoto == null) return;

        // set post photo content
        Sprite postSprite = postPhoto.Image;
        PostImage.sprite = postSprite;

        // set user icons
        Sprite userIcon = user.Icon;
        TitleProfileImage.sprite = userIcon;

        // set text
        TitleUsernameText.text = user.Username;
        PostUsernameText.text = user.Username;
        DescriptionText.text = post.Description;

        // set comments
        foreach(GramCommentScriptableObject comment in post.Comments) {
            GameObject commentObj = Instantiate (
                CommentPrefab,
                CommentsParent
            );

            GramCommentUI commentUI = commentObj.GetComponent<GramCommentUI>();
            commentUI.SetContent(comment, os);
        }
    }

    // ------------------------------------------------------------------------
    public void ToggleOpenComments () {
        CommentsParent.gameObject.SetActive(
            !CommentsParent.gameObject.activeInHierarchy
        );
        LayoutRebuilder.ForceRebuildLayoutImmediate(CommentsParent);
    }
}
