using UnityEngine;
using UnityEngine.UI;

public class GramCommentUI : MonoBehaviour
{
    public Image UserIcon;
    public Text UsernameText;
    public Text CommentText;

    public void SetContent (GramComment comment, PhoneOS os) {
        GramUser user = os.GetGramUser(comment.UserId);
        if(user == null) return;

        UserIcon.sprite = os.GetIcon(user.Icon);
        UsernameText.text = user.Username;
        CommentText.text = comment.Comment;
    }
}