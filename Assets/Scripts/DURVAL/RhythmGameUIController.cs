using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class RhythmGameUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject InputBlocker;
    [SerializeField] private RectTransform getReadyPopup;
    [SerializeField] private RhythmGameSettings rhythmGameSettings;
    [SerializeField] private EndOfLevelScreenController endOfLevelController;

    void Start()
    {
        getReadyPopup.DOScale(Vector3.one, 0.25f).OnComplete(() => {
            getReadyPopup.DOPunchScale(Vector3.one * 0.35f, 0.75f, 7, 0.4f);
        });

        StartCoroutine(CloseInitialPopup());
    }

    private IEnumerator CloseInitialPopup()
    {
        yield return new WaitForSeconds(rhythmGameSettings.delayBeforeLevelStart);
        
        getReadyPopup.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
            getReadyPopup.gameObject.SetActive(false);
            InputBlocker.SetActive(false);
        });
    }

    public void UpdateScoreText(string newValue)
    {
        scoreText.text = newValue;
    }

    public void UpdateComboText(string newValue)
    {
        comboText.text = newValue;
    }

    public void OnLevelEnded(RhythmGameScoreController scoreController){
        endOfLevelController.Activate(scoreController);
    }
}