using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Level1Scripts
{
   public class MusicGuessingLevel : MonoBehaviour
   {
      public GameObject winPanel;
      public GameObject losePanel;
      public GameObject finishPanel;

      public GameObject[] gameLevels;
      public AudioClip[] audioClips;
      private AudioSource _audioSource;
    
      private int _currentLevel;

      private void Start()
      {
         for (int i = 0; i < gameLevels.Length; i++)
         {
            SetUpLevel(i);
            gameLevels[i].gameObject.SetActive(false);
         }
         
         gameLevels[0].gameObject.SetActive(true);

         _audioSource = gameObject.GetComponent<AudioSource>();
         _audioSource.clip = audioClips[_currentLevel];
      
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

      public void SetUpLevel(int levelToSet)
      {
         GameObject topButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
         GameObject bottomButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;
         
         bool isTopCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;
         
         // Set the sprite of the correct button
         if (isTopCorrect)
         {
            topButton.GetComponent<Image>().sprite = Resources.Load("Sprites/correct") as Sprite;
            topButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            bottomButton.GetComponent<Image>().sprite = Resources.Load("Sprites/false") as Sprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
         }
         else
         {
            bottomButton.GetComponent<Image>().sprite = Resources.Load("Sprites/correct") as Sprite;
            bottomButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

            topButton.GetComponent<Image>().sprite = Resources.Load("Sprites/false") as Sprite;
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
         losePanel.SetActive(false);
         gameLevels[_currentLevel].gameObject.SetActive(true);
         _audioSource.Play();
      }
   }
}
