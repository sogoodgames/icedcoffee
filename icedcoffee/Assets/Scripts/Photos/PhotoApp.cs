using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoApp : App
{
    public Transform PhotoGalleryParent;
    public GameObject GalleryTilePrefab;
    public FullscreenImage FullscreenImage;

    public override void Open () {
        base.Open();

        foreach(PhotoScriptableObject photo in PhoneOS.FoundPhotos) {
            GameObject photoObj = Instantiate (
                GalleryTilePrefab,
                PhotoGalleryParent
            );

            PhotoTile photoTile = photoObj.GetComponent<PhotoTile>();
            if(photoTile) {
                //Debug.Log("adding photo: " + photo.Description);
                Sprite img = PhoneOS.GetPhotoSprite(photo.Image);
                photoTile.Image.sprite = null;
                photoTile.Image.preserveAspect = false;
                photoTile.Image.sprite = img;
                photoTile.Image.preserveAspect = true;
                photoTile.Button.onClick.AddListener (
                    delegate {OpenPhoto(img);}
                );
            }
        }
    }

    public override void HandleSlideAnimationFinished () {
        if(m_waitingForClose) {
            foreach(Transform child in PhotoGalleryParent.transform) {
                Destroy(child.gameObject);
            }
        }
        base.HandleSlideAnimationFinished();
    }

    public void OpenPhoto (Sprite sprite) {
        FullscreenImage.Open(sprite);
    }

}