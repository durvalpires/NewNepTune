using System.Collections;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigames
{
   public class MusicGuessingLevel : MonoBehaviour
   {
      [Header("References for Minigame Set Up")]
      public GameObject winPanel;
      public GameObject losePanel;
      public GameObject finishPanel;

      [SerializeField] protected GameObject[] gameLevels;
      [SerializeField] protected AudioClip[] audioClips;
      [SerializeField] protected Sprite[] correctAnswerSprites;
      
      protected AudioSource _audioSource;
      protected Sprite[] _sprites;
      protected int _currentLevel;
      
      [Header("References for Scene Set Up")]
      [SerializeField] protected string levelNote;
      [SerializeField] protected string galaxy;
      [SerializeField] protected Button backButton;
      [SerializeField] protected Button ReloadButton;
      [SerializeField] protected Button finishedBackButton;
      [SerializeField] protected string levelToReturn;
      
      [SerializeField] protected AllGameScoringConfig gameScoringConfig;
      protected GuessScoringSettings _guessScoringSettings;
      protected int score;

        //TODO : get rid of character var and first line of start method
        protected GameObject _character;
      
      protected virtual void Start()
      {
         _character = GameObject.Find("karakter");
         _character.GetComponent<Animator>().Play("RedGirlPiano");
         
         
         if (levelToReturn == "") Debug.LogError("Level to return is not set!");

         backButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         
         _sprites = Resources.LoadAll<Sprite>($"MusicGuessSprites/{levelNote}/");
         
         for (int i = 0; i < gameLevels.Length; i++)
         {
            SetUpLevel(i);
            gameLevels[i].gameObject.SetActive(false);
         }
         gameLevels[0].gameObject.SetActive(true);

         _audioSource = gameObject.GetComponent<AudioSource>();
         _audioSource.clip = audioClips[_currentLevel];
         
         StartCoroutine(PlayAfterSeconds(.8f));
      }

      protected IEnumerator PlayAfterSeconds(float seconds)
      {
         yield return new WaitForSeconds(seconds);
         _audioSource.Play();
      }
      
      public void TrueAnswer()
      {
         score += _guessScoringSettings.maxScore;
         Debug.Log("Score: " + score);
         
         AudioManager.Instance.PlaySFX(SoundList.WinSound);
         gameLevels[_currentLevel].gameObject.SetActive(false);

         if (_currentLevel+1 == gameLevels.Length)
         {
            finishPanel.SetActive(true);
         }
         else
         {
            winPanel.SetActive(true);
         }
      }

      public void FalseAnswer()
      {
         score -= _guessScoringSettings.wrongAnswerPenalty;
         Debug.Log("Score: " + score);
         
         losePanel.SetActive(true);
         gameLevels[_currentLevel].gameObject.SetActive(false);
         AudioManager.Instance.PlaySFX(SoundList.LoseSound);
      }

      protected void SetUpLevel(int levelToSet)
      {
         score = 0;
         
         GameObject topButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
         GameObject bottomButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;

         bool isTopCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;
         
         Sprite randomSprite = correctAnswerSprites[levelToSet];
         while (randomSprite == correctAnswerSprites[levelToSet])
         {
            int randomIndex = Random.Range(0, _sprites.Length);
            randomSprite = _sprites[randomIndex];
         }
         
         // Set the sprite of the correct button
         if (isTopCorrect)
         {
            //Debug.Log($"Top is correct. {levelToSet + 1}");
            topButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            topButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            bottomButton.GetComponent<Image>().sprite = randomSprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
         }
         else
         {
            //Debug.Log($"Bottom is correct. {levelToSet + 1}");
            bottomButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            bottomButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            topButton.GetComponent<Image>().sprite = randomSprite;
            topButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
         }
      }

      public void PlaySoundAgain()
      {
         _audioSource.Stop();
         _audioSource.Play();
      }
      
      public void NextLevelButton()
      {
         if (_currentLevel + 1 < gameLevels.Length)
         {
            _currentLevel++;
            gameLevels[_currentLevel].gameObject.SetActive(true);
            _audioSource.clip = audioClips[_currentLevel];
            winPanel.SetActive(false);
            _audioSource.Play();
         }
         else if (_currentLevel <= gameLevels.Length)
         {
            winPanel.SetActive(false);
            float normalized = (float)score / _guessScoringSettings.maxScore;
            int stars = 0;
            if (normalized >= _guessScoringSettings.threeStarThreshold) stars = 3;
            else if (normalized >= _guessScoringSettings.twoStarThreshold) stars = 2;
            else if (normalized >= _guessScoringSettings.oneStarThreshold) stars = 1;

            int finalScore = Mathf.Max(0, score);
            PlayerModelBase.SetCustomScore(finalScore);
            Debug.Log(finalScore);
            PlayerModelBase.LevelDataService.SetCustomScore(score);
            PlayerModelBase.LevelDataService.SetCustomStarRating(stars);
            finishPanel.SetActive(true);
         }
      }

      public void Restart()
      {
         winPanel.SetActive(false);
         losePanel.SetActive(false);
         gameLevels[_currentLevel].gameObject.SetActive(true);
         _audioSource.Play();
      }
   }
}
