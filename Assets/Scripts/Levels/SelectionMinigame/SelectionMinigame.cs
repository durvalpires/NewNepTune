using System;
using Audio;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Levels.SelectionMinigame
{
    public class SelectionMinigame : MonoBehaviour
    {
        public GameObject winPanel;
        public GameObject losePanel;
        public GameObject finishPanel;

        public GameObject[] gameLevels;

        [Header("References for Scene Set Up")]
        
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private string correctAnswerSpriteName;
        private Sprite[] _sprites;
        private Sprite _correctAnswerSprite;
        
        [SerializeField] private string levelToReturn;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject finishedBackButton;

        private int _currentLevel;

        private void Start()
        {
            Sprite[] answerSprites = Resources.LoadAll<Sprite>($"SelectionMinigame/Empty/");
            Sprite correctSprite = Array.Find(answerSprites, sprite => sprite.name == correctAnswerSpriteName.ToUpper());
            _correctAnswerSprite = correctSprite;
            
            backButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(levelToReturn);
            });
            finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(levelToReturn);
            });
            
            levelText.text = $"Which one is {_correctAnswerSprite.name}?";
            _sprites = Resources.LoadAll<Sprite>($"SelectionMinigame/Empty/");
            
            for (int i = 0; i < gameLevels.Length; i++)
            {
                SetUpLevel(i);
                gameLevels[i].gameObject.SetActive(false);
            }
            gameLevels[0].gameObject.SetActive(true);
        }
        
        private void SetUpLevel(int levelToSet)
        {
            GameObject leftButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
            GameObject rightButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;

            bool isLeftCorrect = System.Guid.NewGuid().GetHashCode() % 2 == 0;

            Sprite randomSprite = _correctAnswerSprite;
            while (randomSprite == _correctAnswerSprite)
            {
                int randomIndex = Random.Range(0, _sprites.Length);
                randomSprite = _sprites[randomIndex];
            }
         
            // Set the sprite of the correct button
            if (isLeftCorrect)
            {
                //Debug.Log($"Left is correct. {levelToSet + 1}");
            
                leftButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                leftButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

                rightButton.GetComponent<Image>().sprite = randomSprite;
                rightButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);    
            }
            else
            {
                //Debug.Log($"Right is correct. {levelToSet + 1}");
            
                rightButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                rightButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

                leftButton.GetComponent<Image>().sprite = randomSprite;
                leftButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
            }
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

        public void NextLevelButton()
        {
            if (_currentLevel + 1 < gameLevels.Length)
            {
                _currentLevel++;
                gameLevels[_currentLevel].gameObject.SetActive(true);
                winPanel.SetActive(false);
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
        }
    }
}
