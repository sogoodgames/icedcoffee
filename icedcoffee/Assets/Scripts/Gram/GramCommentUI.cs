using UnityEngine;
using UnityEngine.UI;

public class GramCommentUI : MonoBehaviour
{
    public Image UserIcon;
    public Text UsernameText;
    public Text CommentText;

    public void SetContent (GramCommentScriptableObject comment, PhoneOS os) {
        GramUserScriptableObject user = os.GetGramUser(comment.UserId);
        if(user == null) return;

        UserIcon.sprite = user.Icon;
        UsernameText.text = user.Username;
        CommentText.text = comment.Comment;
    }
}