using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Audio;
using Enums;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices; // ✅ Needed for DllImport
using _App_v2.Scripts._Core.Firebase;
using UnityEngine.AddressableAssets;

public class SplashScreenController : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int CheckAudioUnlocked();
#endif

    [SerializeField] private Image splashImage;
    [SerializeField] private float fillDuration = 1f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float unfillDuration = 1f;
    [SerializeField] private string nextSceneName = "NeptuneApp";

    private void Start()
    {
        // Initializing FireService early so Firebase is ready before the main app loads
        Debug.Log("[SplashScreen] Triggering FireService initialization...");
        var fireService = FireService.Instance;
        Debug.Log($"[SplashScreen] FireService instance created: {fireService != null}");

        if (splashImage == null)
        {
            Debug.LogError("Splash Image reference is missing!");
            return;
        }

        // Set initial fill amount to 0
        splashImage.fillAmount = 0f;

        // Build the animation sequence
        DOTween.Sequence()
            .AppendInterval(1f)
            .Append(splashImage.DOFillAmount(1f, fillDuration))
            .InsertCallback(1f, () =>
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                // Delay slightly so JS has time to set window.neptuneAudioUnlocked
                Invoke(nameof(PlayFillSoundIfUnlocked), 0.1f);
#else
                PlayLogoIfUnlocked(SoundList.SplashScreenFill);
#endif
            })
            .AppendInterval(displayDuration)
            .Append(splashImage.DOFillAmount(0f, unfillDuration))
            .InsertCallback(fillDuration + displayDuration + 1, () =>
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                Invoke(nameof(PlayUnfillSoundIfUnlocked), 0.1f);
#else
                PlayLogoIfUnlocked(SoundList.SplashScreenUnfill);
#endif
            })
            .AppendInterval(0.2f);
        //.OnComplete(LoadNextScene);
    }

    private void PlayFillSoundIfUnlocked() => PlayLogoIfUnlocked(SoundList.SplashScreenFill);
    private void PlayUnfillSoundIfUnlocked() => PlayLogoIfUnlocked(SoundList.SplashScreenUnfill);

    private void PlayLogoIfUnlocked(SoundList soundToPlay)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        //if (CheckAudioUnlocked() != 0)
        //{
            PlayLogoSound(soundToPlay);
        //}
#else
        PlayLogoSound(soundToPlay);
#endif
    }

    private void PlayLogoSound(SoundList soundToPlay)
    {
        Debug.Log($"Playing logo sound: {soundToPlay}");
        AudioManager.Instance.PlaySFX(soundToPlay);
    }
    
    public void OnAddressablesInitialized(bool success)
    {
        if (success)
        {
            Debug.Log("[SplashScreen] Addressables initialized successfully!");
            LoadNextScene();
        }
        else
        {
            Debug.LogError("[SplashScreen] Failed to initialize Addressables!");
        }
    }

    private void LoadNextScene()
    {
        Debug.Log($"Loading next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }
}