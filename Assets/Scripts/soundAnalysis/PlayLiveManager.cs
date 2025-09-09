using UnityEngine;

public class PlayLiveManager : MonoBehaviour
{
    [SerializeField] private RhythmGameSettings gameSettings; 
    [SerializeField] private GameObject soundAnalysis;        
    [SerializeField] private GameObject mediapipeGameObject;  

    void OnEnable()
    {
        if (gameSettings != null)
        {
            gameSettings.OnPitchControlChanged += Apply;
            Apply(gameSettings.pitchControlEnabled); 
        }
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

        Debug.Log("[PlayLiveManager] Pitch Control " + (enabled ? "ENABLED" : "DISABLED"));
    }
}