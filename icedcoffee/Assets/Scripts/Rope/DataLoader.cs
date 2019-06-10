using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour { 
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public List<TextAsset> ChatTextAssets;
    public List<TextAsset> ForumPostTextAssets;
    public List<TextAsset> GramPostTextAssets;
    public List<TextAsset> GramUserTextAssets;
    public List<TextAsset> ClueTextAssets;
    public List<TextAsset> PhotoTextAssets;
    public List<Sprite> UserIconAssets;
    public List<Sprite> PhotoAssets;
    public Sprite EmmaEndingPhoto;
    public Sprite JinEndingPhoto;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public List<Chat> LoadChats () {
        List<Chat> chats = new List<Chat>();

        foreach(TextAsset textAsset in ChatTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                ChatSerializable chatSer = JsonUtility.FromJson<ChatSerializable>(text);
                Chat chat = new Chat(chatSer);

                if(!chat.HasMessages) {
                    Debug.LogWarning("Chat empty: " + textAsset.name);
                } else {
                    chats.Add(chat);
                    //Debug.Log("added chat: " + chat.friend.ToString() + "; order: " + chat.order);
                }
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return chats;
    }

    // ------------------------------------------------------------------------
    public List<ForumPost> LoadForumPosts() {
        List<ForumPost> posts = new List<ForumPost>();

        foreach(TextAsset textAsset in ForumPostTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                ForumPostSerializable postSer = JsonUtility.FromJson<ForumPostSerializable>(text);
                ForumPost post = new ForumPost(postSer);

                if(post.Empty()) {
                    Debug.LogWarning("Forum Post empty: " + textAsset.name);
                } else {
                    posts.Add(post);
                    //Debug.Log("added post: " + post.Username + "; order: " + post.Order);
                }
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return posts;
    }

    // ------------------------------------------------------------------------
    public List<GramPost> LoadGramPosts() {
        List<GramPost> posts = new List<GramPost>();

        foreach(TextAsset textAsset in GramPostTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                GramPostSerializable postSer = JsonUtility.FromJson<GramPostSerializable>(text);
                GramPost post = new GramPost(postSer);
                posts.Add(post);
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return posts;
    }

    // ------------------------------------------------------------------------
    public List<GramUser> LoadGramUsers() {
        List<GramUser> users = new List<GramUser>();

        foreach(TextAsset textAsset in GramUserTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                GramUserSerializable userSer = JsonUtility.FromJson<GramUserSerializable>(text);
                GramUser user = new GramUser(userSer);
                users.Add(user);
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return users;
    }

    // ------------------------------------------------------------------------
    public List<Clue> LoadClues() {
        List<Clue> clues = new List<Clue>();

        foreach(TextAsset textAsset in ClueTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                ClueSerializable clueSer = JsonUtility.FromJson<ClueSerializable>(text);
                Clue clue = new Clue(clueSer);
                if(!clue.Invalid) {
                    clues.Add(clue);
                } else {
                    Debug.LogError("clue invalid: " + clue.ClueID);
                }
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return clues;
    }

    // ------------------------------------------------------------------------
    public List<Photo> LoadPhotos() {
        List<Photo> photos = new List<Photo>();

        foreach(TextAsset textAsset in PhotoTextAssets) {
            string text = textAsset.text;
            if(!string.IsNullOrEmpty(text)) {
                PhotoSerializable photoSer = JsonUtility.FromJson<PhotoSerializable>(text);
                Photo photo = new Photo(photoSer);
                photos.Add(photo);
            } else {
                Debug.LogError("file empty: " + textAsset.name);
                break;
            }
        }
        return photos;
    }
}