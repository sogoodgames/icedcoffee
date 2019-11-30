using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GramCommentUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Image UserIcon;
    public Text UsernameText;
    public Text CommentText;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void SetContent (GramCommentScriptableObject comment, PhoneOS os) {
        GramUserScriptableObject user = os.GameData.GetGramUser(comment.UserId);
        if(user == null) {
            Assert.IsNotNull(
                user,
                "Gram User" + comment.UserId + " not found."
            );
            return;
        }

        UserIcon.sprite = user.Icon;
        UsernameText.text = user.Username;
        CommentText.text = comment.Comment;
    }
}