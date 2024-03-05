using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Image mainImage; // Merkezdeki Image için bir referans

    // Bu metodu her butonun onClick event'ine bağlayın
    public void ChangeMainImage(Sprite newSprite)
    {
        mainImage.sprite = newSprite;
    }
}
