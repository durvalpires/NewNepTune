using UnityEngine;

public class PlayLiveManager : MonoBehaviour
{
    [SerializeField] private RhythmGameSettings gameSettings; 
    [SerializeField] private GameObject soundAnalysis;        
    [SerializeField] private GameObject mediapipeGameObject;
    [SerializeField] private GameObject Square;
    [SerializeField] private GameObject MusicNode;
    [SerializeField] CanvasGroup VPCanvas;

    void OnEnable()
    {
        if (gameSettings != null)
        {
            gameSettings.OnPitchControlChanged += Apply;
          
        }
    }
    void Start()
    {
        if (gameSettings != null)
            Apply(LivePitchSettings.PitchControlEnabled); 
    }
    void OnDisable()
    {
        if (gameSettings != null)
            gameSettings.OnPitchControlChanged -= Apply;
    }

    private void Apply(bool enabled)
    {
        if (soundAnalysis) soundAnalysis.SetActive(enabled);
        if (mediapipeGameObject) mediapipeGameObject.SetActive(enabled);
        if (Square) Square.SetActive(!enabled);
        if (VPCanvas != null) VPCanvas.alpha = enabled ? 0f : 1f;

        if (enabled && MusicNode != null)
        {
            var cam = Camera.main;
            if (cam != null)
            {
                const float marginPercent = 0.06f; 
                Vector3 vp = cam.WorldToViewportPoint(MusicNode.transform.position);
                vp.y = marginPercent;

                Vector3 worldPos = cam.ViewportToWorldPoint(vp);
                MusicNode.transform.position = new Vector3(
                    MusicNode.transform.position.x,
                    worldPos.y,
                    MusicNode.transform.position.z
                );
            }
        }

        Debug.Log("[PlayLiveManager] Pitch Control " + (enabled ? "ENABLED" : "DISABLED"));
    }
}