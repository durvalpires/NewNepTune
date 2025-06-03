using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class RhythmGameUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image HandImage;
    [SerializeField] private GameObject InputBlocker;
    [SerializeField] private RectTransform getReadyPopup;
    [SerializeField] private TextMeshProUGUI getReadyPopupText;
    [SerializeField] private RhythmGameSettings rhythmGameSettings;
    [SerializeField] private EndOfLevelScreenController endOfLevelController;
    [SerializeField] private RhythmGameManager rhythmGameManager;
    [SerializeField] private MusicScoreRender musicScoreRender;
    void Start()
    {
        if (rhythmGameManager != null && rhythmGameManager.isAutoPlayTutorial)
        {
            StartCoroutine(ShowTutorialSequence());
        }
        else
        {
            ShowGetReadyPopup();
        }
    }
    void OnEnable()
    {
        if (musicScoreRender != null)
            musicScoreRender.OnClefChanged += HandleClefChange;
    }

    void OnDisable()
    {
        if (musicScoreRender != null)
            musicScoreRender.OnClefChanged -= HandleClefChange;
    }
    private void ShowGetReadyPopup()
    {
        getReadyPopup.gameObject.SetActive(true);
        getReadyPopup.localScale = Vector3.zero;
        getReadyPopupText.text = "Get Ready to Play!";
        getReadyPopup.DOScale(Vector3.one, 0.25f).OnComplete(() => {
            getReadyPopup.DOPunchScale(Vector3.one * 0.35f, 0.75f, 7, 0.4f);
        });
        StartCoroutine(CloseInitialPopup());
    }

    private IEnumerator ShowTutorialSequence()
    {
        getReadyPopup.gameObject.SetActive(true);
        InputBlocker.SetActive(true);
        getReadyPopup.localScale = Vector3.zero;
        getReadyPopupText.text = "Welcome to the Tutorial!";
        yield return ScaleAndPunchPopup(0.25f, 0.5f);
        yield return new WaitForSeconds(2f);
        yield return TransitionPopupOut(0.25f);
        getReadyPopupText.text = "1) Watch the notes closely.\n2) Tap or press keys in rhythm!";
        yield return ScaleAndPunchPopup(0.25f, 0.5f);
        yield return new WaitForSeconds(2f);
        yield return TransitionPopupOut(0.25f);
        getReadyPopupText.text = "You'll earn points and build combos\nby staying on beat!";
        yield return ScaleAndPunchPopup(0.25f, 0.5f);
        yield return new WaitForSeconds(2f);
        yield return TransitionPopupOut(0.25f);
        getReadyPopupText.text = "Let's get started and have fun!";
        yield return ScaleAndPunchPopup(0.25f, 0.5f);
        yield return new WaitForSeconds(2f);
        yield return TransitionPopupOut(0.5f);
        getReadyPopup.gameObject.SetActive(false);
        InputBlocker.SetActive(false);
    }

    private IEnumerator CloseInitialPopup()
    {
        yield return new WaitForSeconds(rhythmGameSettings.delayBeforeLevelStart);
        yield return TransitionPopupOut(0.5f);
        getReadyPopup.gameObject.SetActive(false);
        InputBlocker.SetActive(false);
    }

    private IEnumerator ScaleAndPunchPopup(float scaleDuration, float punchDuration)
    {
        bool isScaled = false;
        getReadyPopup.DOScale(Vector3.one, scaleDuration)
            .OnComplete(() =>
            {
                getReadyPopup.DOPunchScale(Vector3.one * 0.35f, punchDuration, 7, 0.4f);
                isScaled = true;
            });
        while (!isScaled)
            yield return null;
    }
   
    private IEnumerator TransitionPopupOut(float duration)
    {
        bool isDone = false;
        getReadyPopup.DOScale(Vector3.zero, duration)
            .OnComplete(() => { isDone = true; });
        while (!isDone)
            yield return null;
    }

    public void UpdateScoreText(string newValue)
    {
        scoreText.text = newValue;
    }

    public void UpdateComboText(string newValue)
    {
        comboText.text = newValue;
    }
    private void HandleClefChange(HandType handType)
    {
        HandImage.sprite = rhythmGameSettings.GetHandSprite(handType);
        HandImage.enabled = true;
        Vector3 scale = HandImage.transform.localScale;
        HandImage.transform.localScale = handType == HandType.Right
            ? new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z)
            : new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
    }
    public void OnLevelEnded(RhythmGameScoreController scoreController)
    {
        endOfLevelController.Activate(scoreController);
    }
}
