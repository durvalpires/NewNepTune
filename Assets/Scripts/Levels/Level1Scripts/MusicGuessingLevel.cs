using System.Collections;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Level1Scripts
{
   public class MusicGuessingLevel : MonoBehaviour
   {
      public string levelNote;
      
      public GameObject winPanel;
      public GameObject losePanel;
      public GameObject finishPanel;

      [SerializeField] private GameObject[] gameLevels;
      [SerializeField] private AudioClip[] audioClips;
      [SerializeField] private Sprite[] correctAnswerSprites;
      private AudioSource _audioSource;
    
      private Sprite[] _sprites;
    
      private int _currentLevel;

      private void Start()
      {
         _sprites = Resources.LoadAll<Sprite>("InstrumentPNGs");
         
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

      private IEnumerator PlayAfterSeconds(float seconds)
      {
         yield return new WaitForSeconds(seconds);
         _audioSource.Play();
      }
      
      public void TrueAnswer()
      {
         winPanel.SetActive(true);
         AudioManager.Instance.PlaySFX(SoundList.WinSound);
         gameLevels[_currentLevel].gameObject.SetActive(false);
      }

      public void FalseAnswer()
      {
         losePanel.SetActive(true);
         gameLevels[_currentLevel].gameObject.SetActive(false);
         AudioManager.Instance.PlaySFX(SoundList.LoseSound);
      }

      private void SetUpLevel(int levelToSet)
      {
         GameObject topButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
         GameObject bottomButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;

         bool isTopCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;

         //int randomIndex = Random.Range(0, _sprites.Length);
         //Sprite randomSprite = _sprites[randomIndex];

         Sprite randomSprite = correctAnswerSprites[levelToSet];
         while (randomSprite == correctAnswerSprites[levelToSet])
         {
            int randomIndex = Random.Range(0, _sprites.Length);
            randomSprite = _sprites[randomIndex];
         }
         
         // Set the sprite of the correct button
         if (isTopCorrect)
         {
            Debug.Log($"Top is correct. {levelToSet + 1}");
            
            topButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            topButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            bottomButton.GetComponent<Image>().sprite = randomSprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
         }
         else
         {
            Debug.Log($"Bottom is correct. {levelToSet + 1}");
            
            bottomButton.GetComponent<Image>().sprite = correctAnswerSprites[levelToSet];
            bottomButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            topButton.GetComponent<Image>().sprite = randomSprite;
            topButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
         }
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
