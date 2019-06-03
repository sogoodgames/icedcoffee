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

        foreach(Photo photo in PhoneOS.FoundPhotos) {
            GameObject photoObj = Instantiate (
                GalleryTilePrefab,
                PhotoGalleryParent
            );

            PhotoTile photoTile = photoObj.GetComponent<PhotoTile>();
            if(photoTile) {
                //Debug.Log("adding photo: " + photo.Description);
                Sprite img = PhoneOS.GetPhotoSprite(photo.Image);
                photoTile.Image.sprite = img;
                photoTile.Button.onClick.AddListener (
                    delegate {OpenPhoto(img);}
                );
            }
        }
    }

    public void OpenPhoto (Sprite sprite) {
        FullscreenImage.Open(sprite);
    }

}