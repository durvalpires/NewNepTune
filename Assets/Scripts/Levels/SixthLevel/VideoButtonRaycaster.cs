using UnityEngine;
using UnityEngine.Video;

public class VideoButtonRaycaster : MonoBehaviour
{
    public Camera cam; // Kamera atanmalı, genellikle ana kamera.

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol mouse butonuna basıldığında
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                VideoPlayer vp = hit.transform.GetComponent<VideoPlayer>();
                if (vp != null)
                {
                    if (vp.isPlaying)
                    {
                        vp.Pause();
                    }
                    else
                    {
                        vp.Play();
                    }
                }
            }
        }
    }
}



