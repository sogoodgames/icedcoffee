using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactsApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public Transform ContactsParent;
    public GameObject ContactPrefab;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public override void Open () {
        base.Open();
        PopulateContacts();
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            foreach(Transform child in ContactsParent.transform) {
                Destroy(child.gameObject);
            }
        }
        base.HandleSlideAnimationFinished();
    }

    // ------------------------------------------------------------------------
    private void PopulateContacts () {
        foreach(FriendScriptableObject friend in PhoneOS.ActiveFriends) {
            GameObject contactObj = Instantiate(ContactPrefab, ContactsParent);

            ContactUI contactUI = contactObj.GetComponent<ContactUI>();
            if(contactUI) {
                GramUserScriptableObject gram = PhoneOS.GameData.GetGramUser(friend.Friend);
                MusicUserScriptableObject music = PhoneOS.GameData.GetMusicUser(friend.Friend);
                ForumUserScriptableObject forum = PhoneOS.GameData.GetForumUser(friend.Friend);
                contactUI.SetContent (
                    name:friend.Name,
                    gram:gram != null ? gram.Username : "",
                    music:music != null ? music.Username : "",
                    forum:forum != null ? forum.Username : "",
                    icon:friend.Icon
                );
            }
        }
    }

}
