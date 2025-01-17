using UnityEngine;

public class CameraSizeAdjuster : MonoBehaviour
{
    [SerializeField] private Camera targetCam;
    
    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        if (targetCam == null) {
            targetCam = GetComponent<Camera>();
            if(targetCam == null) {
                targetCam = Camera.main;
            }
        }

        if (targetCam == null)
        {
            Debug.LogWarning("No Main Camera found to adjust.");
            return;
        }

        if (IsMobilePhone())
        {
            Debug.Log("ISSA MOBILE PHONE");
            targetCam.orthographicSize = 5f;
        }
    }

    bool IsMobilePhone()
    {
        float screenWidthInches = Screen.width / Screen.dpi;
        float screenHeightInches = Screen.height / Screen.dpi;
        float screenSizeInches = Mathf.Sqrt(screenWidthInches * screenWidthInches + screenHeightInches * screenHeightInches);

        // Devices with a screen size less than 7 inches are considered phones.
        return screenSizeInches > 0 && screenSizeInches < 7f;
    }
}