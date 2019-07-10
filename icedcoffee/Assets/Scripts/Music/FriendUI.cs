using UnityEngine;
using UnityEngine.UI;

public class FriendUI : MonoBehaviour 
{
    public Text UsernameText;
    public Button Button;

    public void SetFriendContent (string name, MusicUserId userID, MusicApp musicApp) {
        UsernameText.text = name;
        Button.onClick.AddListener(
            delegate { musicApp.OpenPlaylist(userID); }
        );
    }
}