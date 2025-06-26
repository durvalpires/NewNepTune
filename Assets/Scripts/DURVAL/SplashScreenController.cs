using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Audio;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private Image splashImage;
    [SerializeField] private float fillDuration = 1f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float unfillDuration = 1f;
    [SerializeField] private string nextSceneName = "NeptuneApp";

    private void Start()
    {
        if (splashImage == null)
        {
            Debug.LogError("Splash Image reference is missing!");
            return;
        }

        // Set initial fill amount to 0
        splashImage.fillAmount = 0f;

        // Sequence of animations
        DOTween.Sequence()
            .AppendInterval(1f)
            .Append(splashImage.DOFillAmount(1f, fillDuration))
            .InsertCallback(1f, () => AudioManager.Instance.PlaySFX(Enums.SoundList.SplashScreenFill))
            .AppendInterval(displayDuration)
            .Append(splashImage.DOFillAmount(0f, unfillDuration))
            .InsertCallback(fillDuration + displayDuration + 1, () => AudioManager.Instance.PlaySFX(Enums.SoundList.SplashScreenUnfill))
            .AppendInterval(0.2f)
            .OnComplete(LoadNextScene);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
        Debug.Log(nextSceneName);
    }
}
