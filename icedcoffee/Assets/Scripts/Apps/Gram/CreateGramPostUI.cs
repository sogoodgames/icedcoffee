using System;

using UnityEngine;
using UnityEngine.UI;

public class CreateGramPostUI : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public GramApp GramApp;
    public InputField CaptionInputField;
    public Transform ImageTilesParent;
    public Button UploadButton;
    public GameObject GalleryTilePrefab;

    private PhotoScriptableObject InputPhoto;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void Update () {
        UploadButton.interactable = !(
            InputPhoto == null ||
            string.IsNullOrEmpty(CaptionInputField.text)
        );    
    }

    // ------------------------------------------------------------------------
    public void Open () {
        foreach(PhotoScriptableObject photo in GramApp.PhoneOS.FoundPhotos) {
            GameObject photoObj = Instantiate (
                GalleryTilePrefab,
                ImageTilesParent
            );

            PhotoTile photoTile = photoObj.GetComponent<PhotoTile>();
            if(photoTile) {
                Sprite img = photo.Image;
                photoTile.Image.sprite = null;
                photoTile.Image.preserveAspect = false;
                photoTile.Image.sprite = img;
                photoTile.Image.preserveAspect = true;

                photoTile.Button.onClick.AddListener (
                    delegate {SetInputImage(photo);}
                );
            }
        }

        gameObject.SetActive(true);
    }

    // ------------------------------------------------------------------------
    public void Close () {
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------
    public void SetInputImage (PhotoScriptableObject photo) {
        InputPhoto = photo;
    }

    // ------------------------------------------------------------------------
    public void CreatePost () {
        if(InputPhoto == null ||
            string.IsNullOrEmpty(CaptionInputField.text)
        ) {
            return;
        }

        // create gram post
        GramPostScriptableObject postSO = new GramPostScriptableObject();
        postSO.CreatePlayerPost (
            CaptionInputField.text,
            InputPhoto,
            DateTime.Now.Ticks
        );

        // add to progression
        GramApp.PhoneOS.CreateGramPost(postSO);

        // move to feed page, showing your new post
        GramApp.OpenFeed();
    }
}
