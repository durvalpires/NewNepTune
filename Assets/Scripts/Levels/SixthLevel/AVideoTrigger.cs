using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class AVideoTrigger : MonoBehaviour, IPointerClickHandler
{
    public VideoPlayer videoPlayer; // Inspector üzerinden atanmalı

    public void OnPointerClick(PointerEventData eventData)
    {
        // VideoPlayer kontrolü
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause(); // Video oynatılıyorsa duraklat
        }
        else
        {
            videoPlayer.Play(); // Video oynatılmıyorsa oynat
        }
    }
}
