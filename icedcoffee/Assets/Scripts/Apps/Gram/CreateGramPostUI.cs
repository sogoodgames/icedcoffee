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

    private PhotoID InputPhotoID;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    void Update () {
        UploadButton.interactable = !(
            InputPhotoID == PhotoID.NoPhoto ||
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
                    delegate {SetInputImage(photo.PhotoID);}
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
    public void SetInputImage (PhotoID id) {
        InputPhotoID = id;
    }

    // ------------------------------------------------------------------------
    public void CreatePost () {
        if(InputPhotoID == PhotoID.NoPhoto ||
            string.IsNullOrEmpty(CaptionInputField.text)
        ) {
            return;
        }

        // create gram post
        GramPostScriptableObject postSO = new GramPostScriptableObject();
        postSO.SetupPlayerPost (
            CaptionInputField.text,
            InputPhotoID
        );

        // add to progression
        GramApp.PhoneOS.CreateGramPost(postSO);

        // move to feed page, showing your new post
        GramApp.OpenFeed();
    }
}
