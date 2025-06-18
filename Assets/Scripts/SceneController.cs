using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] protected Sprite[] images;
    [SerializeField] private TMP_Text scoreLabel;
    
    public GameObject finishPanel;
    [SerializeField] private Button backButton;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    [SerializeField] private AllGameScoringConfig gameScoringConfig;
    [SerializeField] private ILevelDataService levelDataService;
  

    private int _score = 0;
    private int _totalPairs;
    private int _pointsPerMatch;

    public bool CanReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
        
    }

    private IEnumerator CheckMatch()
    {
        var memConfig = gameScoringConfig.memoryScoring;

        if (_firstRevealed.Id == _secondRevealed.Id)
        {
            _score += _pointsPerMatch;
            yield return new WaitForSeconds(1f);
            _firstRevealed.gameObject.SetActive(false);
            _secondRevealed.gameObject.SetActive(false);
        }
        else
        {
            _score = Mathf.Max(0, _score - memConfig.mismatchPenalty);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
            yield return new WaitForSeconds(.5f);
        }

        scoreLabel.text = $"Score: {_score}";
        _firstRevealed = null;
        _secondRevealed = null;

        if (_score > memConfig.maxScore) 
            _score = memConfig.maxScore;

        if (_score == memConfig.maxScore)
        {
            backButton.interactable = false;
            StartCoroutine(GameCompleted());
        }
    }

    private IEnumerator GameCompleted()
    {
        AudioManager.Instance.PlaySFX(SoundList.WinSound);
        yield return new WaitForSeconds(1.5f);

        var memConfig = gameScoringConfig.memoryScoring;
        float normalized = (float)_score / memConfig.maxScore;
        int stars = 0;
        if (normalized >= memConfig.threeStarThreshold) stars = 3;
        else if (normalized >= memConfig.twoStarThreshold) stars = 2;
        else if (normalized >= memConfig.oneStarThreshold) stars = 1;

       
        levelDataService.SetCustomScore(_score);
        levelDataService.SetCustomStarRating(stars);
        //levelDataService.SetLevelCompleted(_currentLevelIndex);

        finishPanel.SetActive(true);
    }
    
    void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);
                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
        _totalPairs = images.Length;  
        var memConfig = gameScoringConfig.memoryScoring;
        _pointsPerMatch = memConfig.maxScore / _totalPairs;

    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
