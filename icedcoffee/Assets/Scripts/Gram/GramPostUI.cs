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
    public Transform CommentsParent;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetPostContent (GramPost post, PhoneOS os) {
        GramUser user = os.GetGramUser(post.UserId);
        if(user == null) return;
        Photo postPhoto = os.GetPhoto(post.PostImage);
        if(postPhoto == null) return;

        // set post photo content
        Sprite postSprite = os.GetPhotoSprite(postPhoto.Image);
        PostImage.sprite = postSprite;

        // set user icons
        Sprite userIcon = os.GetIcon(user.Icon);
        TitleProfileImage.sprite = userIcon;

        // set text
        TitleUsernameText.text = user.Username;
        PostUsernameText.text = user.Username;
        DescriptionText.text = post.Description;

        // set comments
        foreach(GramComment comment in post.Comments) {
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
    }
}
