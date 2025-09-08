using Audio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScreenController : MonoBehaviour
{
    [SerializeField] private RectTransform ScoreTextContainer;
    [SerializeField] private TextMeshProUGUI ScoreAmountText;
    [SerializeField] private RectTransform StarsAmountContainer;
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Sprite starFilledSprite;
    [SerializeField] private float scoreAnimationInSec = 1;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelResultText;
    [SerializeField] RhythmPitchGate pitchGate;

    void Awake()
    {
        var data = TempDataStorage.GetSceneData<VirtualPianoLevelSO>();
        if (data == null)
        {
            levelName.text = "Test Level";
        }
        else
        {
            levelName.text = data.levelTitle;
        }
    }
    private void OnEnable()
    {
        pitchGate?.OnGameEnded();
    }

    public void Activate(RhythmGameScoreController scoreController){
        this.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(Enums.SoundList.EndOfRhythmLevel);

        var currentScore = 0;
        int starsAchieved = 0;
        bool star1Awarded = false;
        bool star2Awarded = false;
        bool star3Awarded = false;

        if (scoreController.PlayerStars > 0)
        {
            GetComponent<LevelCompletObserver>().SetCurrentLevelComplete(scoreController);
            levelResultText.text = "Level Complete!";
        }
        else
        {
            // MAYBE WE NEED AN EQUIVALENT FOR WHEN THE PLAYER LOSES
            GetComponent<LevelCompletObserver>().SetCurrentLevelComplete(scoreController);
            levelResultText.text = "Almost! Try again!";
        }
        

        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(
            // Use DOTween to animate from the currentScore to targetScore
            DOTween.To(() => currentScore, x => currentScore = x, scoreController.PlayerScore, 
                    scoreAnimationInSec)
            .OnStart(() => {
                ScoreAmountText.rectTransform.DOShakePosition(scoreAnimationInSec, 0.5f, 10, 90,
                    true);
                AudioManager.Instance.PlaySFX(Enums.SoundList.ScoreCount);
            })
            .OnUpdate(() => ScoreAmountText.text = currentScore.ToString())  // Update text every step
            .OnComplete(() => Debug.Log("Score animation complete!"))  // Optional: Action on completion
        );
        sequence.Append(
            DOTween.To(() => starsAchieved, x => starsAchieved = x, scoreController.PlayerStars, 
                    scoreAnimationInSec)
            .OnUpdate(() => {
                if (!star1Awarded &&starsAchieved == 1) {
                    star1Awarded = true;
                    star1.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                    star1.sprite = starFilledSprite;
                    AudioManager.Instance.PlaySFX(Enums.SoundList.Star1Achieved);
                }
                if (!star2Awarded && starsAchieved == 2) {
                    star2Awarded = true;
                    star2.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                    star2.sprite = starFilledSprite;
                    AudioManager.Instance.PlaySFX(Enums.SoundList.Star2Achieved);
                }
                if (!star3Awarded && starsAchieved == 3) {
                    star3Awarded = true;
                    star3.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                    star3.sprite = starFilledSprite;
                    AudioManager.Instance.PlaySFX(Enums.SoundList.Star3Achieved);
                }
            })
        );
    }
   
}
