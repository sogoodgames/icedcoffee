using UnityEngine;
using UnityEngine.UI;

public class FriendListUI : MonoBehaviour 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public PhoneOS PhoneOS;
    public MusicApp MusicApp;
    public Transform FriendListParent;
    public GameObject FriendPrefab;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Open () {
        // populate list of friends
        foreach(MusicUser user in PhoneOS.ActiveMusicUsers) {
            GameObject userObj = Instantiate (
                FriendPrefab,
                FriendListParent
            );

            FriendUI friendUI = userObj.GetComponent<FriendUI>();
            friendUI.SetFriendContent(user.Username, user.UserID, MusicApp);
        }

        gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public void Close () {
        foreach(Transform child in FriendListParent.transform) {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}