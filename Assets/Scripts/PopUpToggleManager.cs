using UnityEngine;

public class PopUpToggleManager : MonoBehaviour
{
    public GameObject popup1; // Kontrol etmek istediğiniz GameObject
    public GameObject popup2;
    public GameObject popup3;

    public void TogglePopup1()
    {
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popup1.SetActive(!popup1.activeSelf);
    }
    public void TogglePopup2()
    {
        popup2.SetActive(!popup2.activeSelf);
    }
    public void TogglePopup3()
    {
        popup3.SetActive(!popup3.activeSelf);
    }
}
