﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForumApp : App
{
    public Transform ForumPostParent;
    public GameObject ForumPostPrefab;

    public override void Open() {
        base.Open();

        foreach(ForumPost post in PhoneOS.ActiveForumPosts) {
            GameObject postObj = Instantiate(
                ForumPostPrefab,
                ForumPostParent)
            as GameObject;

            ForumPostUI postUI = postObj.GetComponent<ForumPostUI>();
            if(postUI) {
                // set all basic info
                postUI.TitleText.text = post.Title;
                postUI.UsernameText.text = "u/" + post.Username;
                postUI.MetaInfoText.text = post.NumComments
                                        + " comments / posted "
                                        + post.Time
                                        + " hours ago";
                postUI.BodyText.text = post.Body;

                // load profile icon
                Sprite icon = PhoneOS.GetIcon(post.Icon);
                if(icon) {
                    postUI.ProfileImage.sprite = icon;
                }

                // load post image
                postUI.SetPhotoContent(post, PhoneOS);

                // let phone OS know when we encounter this post
                postUI.OpenPostButton.onClick.AddListener(
                    delegate{PhoneOS.FoundClue(post.ClueGiven);}
                );
            }
        }
    }

    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            foreach(Transform t in ForumPostParent.transform) {
                Destroy(t.gameObject);
            }
        }
        base.HandleSlideAnimationFinished();
    }
}