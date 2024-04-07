using System.Collections;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigames
{
   public class InstrumentGuess : MonoBehaviour
   {
      [Header("References for Scene Set Up")]
      [SerializeField] private Button backButton;
      [SerializeField] private Button finishedBackButton;
      [SerializeField] private string levelToReturn;

      [Header("References for Minigame Set Up")]
      public GameObject winPanel;
      public GameObject losePanel;
      public GameObject finishPanel;

      [SerializeField] private GameObject[] gameLevels;
      [SerializeField] private AudioClip[] audioClips;
      [SerializeField] private Sprite[] correctAnswerSprites;
      [SerializeField] private GameObject character;

      private AudioSource _audioSource;
      private Sprite[] _sprites;
      private int _currentLevel;
      
      private void Start()
      {
         character.GetComponent<Animator>().Play($"BoyAst{correctAnswerSprites[_currentLevel].name}");
         
         backButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         
         _sprites = Resources.LoadAll<Sprite>("InstrumentPNGs");

         for (int i = 0; i < gameLevels.Length; i++)
         {
            SetUpLevel(i);
            gameLevels[i].gameObject.SetActive(false);
         }
         gameLevels[0].gameObject.SetActive(true);
         
         _audioSource = gameObject.GetComponent<AudioSource>();
         _audioSource.clip = audioClips[_currentLevel];
         
         StartCoroutine(PlayAfterSeconds());
      }

      private IEnumerator PlayAfterSeconds()
      {
         yield return new WaitForSeconds(.8f);
         _audioSource.Play();
      }
      
      private void TrueAnswer()
      {
         StartCoroutine(TrueAnswerRoutine());
      }
      
      private IEnumerator TrueAnswerRoutine()
      {
         InstrumentGuessClouds.Instance.DisperseClouds();
         
         yield return new WaitForSeconds(3f);

         winPanel.SetActive(true);
         AudioManager.Instance.PlaySFX(SoundList.WinSound);
         gameLevels[_currentLevel].gameObject.SetActive(false);
      }

      private IEnumerator FalseAnswerRoutine()
      {
         InstrumentGuessClouds.Instance.DisperseClouds();
         yield return new WaitForSeconds(3f);
         
         losePanel.SetActive(true);
         gameLevels[_currentLevel].gameObject.SetActive(false);
         AudioManager.Instance.PlaySFX(SoundList.LoseSound);
      }

      private void FalseAnswer()
      {
         StartCoroutine(FalseAnswerRoutine());
      }

      private void SetUpLevel(int levelToSet)
      {
         GameObject topButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
         GameObject bottomButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;
      
         bool isLeftCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;
      
         //int randomIndex = Random.Range(0, _sprites.Length);
         //Sprite randomSprite = _sprites[randomIndex];
      
         Sprite randomSprite = correctAnswerSprites[levelToSet];
         while (randomSprite == correctAnswerSprites[levelToSet])
         {
            int randomIndex = Random.Range(0, _sprites.Length);
            randomSprite = _sprites[randomIndex];
         }
         
         // Set the sprite of the correct button
         if (isLeftCorrect)
         {
            //Debug.Log($"Left is correct. {levelToSet + 1}");
            
            topButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            topButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
      
            bottomButton.GetComponent<Image>().sprite = randomSprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
         }
         else
         {
            //Debug.Log($"Right is correct. {levelToSet + 1}");
            
            bottomButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            bottomButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
      
            topButton.GetComponent<Image>().sprite = randomSprite;
            topButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
         }
      }

      public void NextLevelButton()
      {
         StartCoroutine(NextLevelButtonRoutine());
      }
      
      private IEnumerator NextLevelButtonRoutine()
      {
         if (_currentLevel + 1 < gameLevels.Length)
         {
            _currentLevel++;
            gameLevels[_currentLevel].gameObject.SetActive(true);
            _audioSource.clip = audioClips[_currentLevel];
            winPanel.SetActive(false);
            
            InstrumentGuessClouds.Instance.ClusterClouds();
            yield return new WaitForSeconds(3f);
            character.GetComponent<Animator>().Play($"BoyAst{correctAnswerSprites[_currentLevel].name}");

            _audioSource.Play();
         }
         else if (_currentLevel <= gameLevels.Length)
         {
            winPanel.SetActive(false);
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
