using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GramApp : App
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    // feed
    public GameObject Feed;
    public Transform FeedPostsParent;

    // profile
    public GameObject Profile;
    public Transform ProfilePostsParent;
    public Text ProfileUsernameText;
    public Text ProfileStatsText;
    public Text ProfileDescriptionText;
    public Image ProfileIcon;

    // create post
    public CreateGramPostUI CreatePost;

    // prefabs
    public GameObject GramPostPrefab;

    private Friend m_activeFriend = Friend.NoFriend;

    // ------------------------------------------------------------------------
    // Methods: App
    // ------------------------------------------------------------------------
    public override void Open () {
        base.Open();
        OpenFeed();

        m_activeFriend = Friend.NoFriend;

        PhoneOS.ReturnButton.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            CloseFeed();
            CloseProfile();
        }
        base.HandleSlideAnimationFinished();
    }

    // ------------------------------------------------------------------------
    public override void Return() {
        if(Feed.gameObject.activeInHierarchy) {
            if(m_activeFriend != Friend.NoFriend) {
                OpenProfile(m_activeFriend);
            } else {
                Close();
            }
        } else {
            OpenFeed();
        }
    }

    // ------------------------------------------------------------------------
    // Methods: Gram
    // ------------------------------------------------------------------------
    public void OpenFeed () {
        CloseProfile();
        CloseCreatePost();
        Feed.SetActive(true);

        foreach(GramPostScriptableObject post in PhoneOS.ActiveGramPosts) {
            GameObject postObj = Instantiate(
                GramPostPrefab,
                FeedPostsParent
            );

            GramPostUI postUI = postObj.GetComponent<GramPostUI>();
            postUI.SetPostContent(post, this);

            postUI.ProfileButton.onClick.AddListener (
                delegate {OpenProfile(post.UserId);}
            );
        }
    }

    // ------------------------------------------------------------------------
    public void CloseFeed () {
        foreach(Transform child in FeedPostsParent.transform) {
            Destroy(child.gameObject);
        }
        Feed.SetActive(false);
    }

    // ------------------------------------------------------------------------
    public void OpenPlayerProfile () {
        OpenProfile(Friend.You);
    }

    // ------------------------------------------------------------------------
    public void OpenProfile (Friend friend) {
        CloseFeed();
        CloseCreatePost();
        Profile.SetActive(true);

        m_activeFriend = friend;

        GramUserScriptableObject gramUser = PhoneOS.GameData.GetGramUser(friend);

        int posts = 0;
        foreach(GramPostScriptableObject post in PhoneOS.ActiveGramPosts) {
            if(post.UserId != friend) {
                continue;
            }
            posts ++;

            GameObject postObj = Instantiate(
                GramPostPrefab,
                ProfilePostsParent
            );

            GramPostUI postUI = postObj.GetComponent<GramPostUI>();
            postUI.SetPostContent(post, this);
        }

        ProfileUsernameText.text = gramUser.Username;
        ProfileStatsText.text = posts + " posts - " + gramUser.NumFollowers + " followers - " + gramUser.NumFollowing + " following";
        ProfileDescriptionText.text = gramUser.Description;
        ProfileIcon.sprite = gramUser.Icon;
    }

    // ------------------------------------------------------------------------
    public void CloseProfile () {
        foreach(Transform child in ProfilePostsParent.transform) {
            Destroy(child.gameObject);
        }
        Profile.SetActive(false);
    }

    // ------------------------------------------------------------------------
    public void OpenCreatePost () {
        CloseFeed();
        CloseProfile();
        CreatePost.gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public void CloseCreatePost () {
        CreatePost.gameObject.SetActive(false);
    }
}