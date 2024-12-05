using System.Collections;
using System.Linq;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Minigames
{
   public class InstrumentGuess : MonoBehaviour
   {
      [Header("References for Scene Set Up")]
      [SerializeField] protected string levelInstrument;
      [SerializeField] protected Button backButton;
      [SerializeField] protected Button finishedBackButton;
      [SerializeField] public string levelToReturn;

      [Header("References for Minigame Set Up")]
      public GameObject winPanel;
      public GameObject losePanel;
      public GameObject finishPanel;

      [SerializeField] protected GameObject[] gameLevels;
      //protected Sprite[] _correctAnswerSprites;
      protected Sprite[] _correctAnswerSprites;
      protected AudioClip[] _correctAudioClips;
      
      [SerializeField] protected GameObject character;

      protected AudioSource _audioSource;
      protected Sprite[] _sprites;
      protected int _currentLevel;
      
      protected virtual void Start()
      {  
         backButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
         {
            SceneManager.LoadScene(levelToReturn);
         });
         
         _sprites = Resources.LoadAll<Sprite>($"InstrumentPNGs");

         for (int i = 0; i < gameLevels.Length; i++)
         {
            SetUpLevel(i);
            gameLevels[i].gameObject.SetActive(false);
         }
         gameLevels[0].gameObject.SetActive(true);
         
         _audioSource = gameObject.GetComponent<AudioSource>();
         _audioSource.clip = _correctAudioClips[_currentLevel];
         
         StartCoroutine(PlayAfterSeconds(0.8f));
      }

      protected IEnumerator PlayAfterSeconds(float seconds)
      {
         yield return new WaitForSeconds(seconds);
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

         //winPanel.SetActive(true);
         AudioManager.Instance.PlaySFX(SoundList.WinSound);
         gameLevels[_currentLevel].gameObject.SetActive(false);

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

      private void FalseAnswer()
      {
         StartCoroutine(FalseAnswerRoutine());
      }

      private IEnumerator FalseAnswerRoutine()
      {
         //InstrumentGuessClouds.Instance.DisperseClouds();
         yield return new WaitForSeconds(.25f);
         
         losePanel.SetActive(true);
         gameLevels[_currentLevel].gameObject.SetActive(false);
         AudioManager.Instance.PlaySFX(SoundList.LoseSound);
      }

      
      protected void SetUpLevel(int levelToSet)
      {
         GameObject topButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
         GameObject bottomButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;
         
         topButton.GetComponent<Button>().onClick.RemoveAllListeners();
         bottomButton.GetComponent<Button>().onClick.RemoveAllListeners();
         
         bool isLeftCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;
      
         //int randomIndex = Random.Range(0, _sprites.Length);
         //Sprite randomSprite = _sprites[randomIndex];
      
         Sprite randomSprite = _correctAnswerSprites[levelToSet];
         while (randomSprite == _correctAnswerSprites[levelToSet])
         {
            int randomIndex = Random.Range(0, _sprites.Length);
            randomSprite = _sprites[randomIndex];
         }
         
         // Set the sprite of the correct button
         if (isLeftCorrect)
         {
            //Debug.Log($"Left is correct. {levelToSet + 1}");
            
            topButton.GetComponent<Image>().sprite = _correctAnswerSprites[levelToSet];
            topButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
      
            bottomButton.GetComponent<Image>().sprite = randomSprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
         }
         else
         {
            //Debug.Log($"Right is correct. {levelToSet + 1}");
            
            bottomButton.GetComponent<Image>().sprite = _correctAnswerSprites[levelToSet];
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
            _audioSource.clip = _correctAudioClips[_currentLevel];
            winPanel.SetActive(false);
            
            GameObject topButton = gameLevels[_currentLevel].transform.GetChild(0).gameObject;
            GameObject bottomButton = gameLevels[_currentLevel].transform.GetChild(1).gameObject;
            
            
            topButton.GetComponent<Button>().interactable = false;
            bottomButton.GetComponent<Button>().interactable = false;
            
            InstrumentGuessClouds.Instance.ClusterClouds();
            yield return new WaitForSeconds(3f);
            
            topButton.GetComponent<Button>().interactable = true;
            bottomButton.GetComponent<Button>().interactable = true;
            
            character.GetComponent<Animator>().Play($"BoyAst{_correctAnswerSprites[_currentLevel].name}");
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
         SetUpLevel(_currentLevel);
         InstrumentGuessClouds.Instance.ClusterClouds();
         gameLevels[_currentLevel].gameObject.SetActive(true);
         _audioSource.Play();
      }
   }
}
