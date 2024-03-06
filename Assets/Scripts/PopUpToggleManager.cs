using Audio;
using Enums;
using UnityEngine;

public class PopUpToggleManager : MonoBehaviour
{
    public GameObject popup1; // Kontrol etmek istediğiniz GameObject
    public GameObject popup2;
    public GameObject popup3;
    public GameObject popUpCanvas;
    
    public void TogglePopup1()
    {
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popUpCanvas.SetActive(true);
        popup1.SetActive(!popup1.activeSelf);
        AudioManager.Instance.PlaySFX(SoundList.note_C);
    }
    public void TogglePopup2()
    {
        popUpCanvas.SetActive(true);
        popup2.SetActive(!popup2.activeSelf);
    }
    public void TogglePopup3()
    {
        popUpCanvas.SetActive(true);
        popup3.SetActive(!popup3.activeSelf);
    }
}
