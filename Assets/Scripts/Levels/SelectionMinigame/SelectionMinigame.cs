using System;
using System.Collections.Generic;
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
        protected Sprite[] _sprites;
        private Sprite _correctAnswerSprite;
        
        public string correctAnswerSpriteName;
        public string levelToReturn;
        protected string contentType;
        
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject finishedBackButton;

        private int _currentLevel;

        private void Start()
        {
            //Sprite[] answerSprites = Resources.LoadAll<Sprite>($"SelectionMinigame/Notes/");
            Sprite correctSprite = Array.Find(_sprites, sprite => sprite.name == correctAnswerSpriteName.ToUpper());
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
            //_sprites = Resources.LoadAll<Sprite>($"SelectionMinigame/Empty/");
            
            for (int i = 0; i < gameLevels.Length; i++)
            {
                SetUpLevel(i);
                gameLevels[i].gameObject.SetActive(false);
            }
            gameLevels[0].gameObject.SetActive(true);
        }
        
        private void SetUpLevel(int levelToSet)
        {
            GameObject firstButton = gameLevels[levelToSet].transform.GetChild(0).gameObject;
            GameObject secondButton = gameLevels[levelToSet].transform.GetChild(1).gameObject;
            GameObject thirdButton = null;
            GameObject fourthButton = null;
            
            if (levelToSet > 1)
            {
                thirdButton = gameLevels[levelToSet].transform.GetChild(2).gameObject;
                fourthButton = gameLevels[levelToSet].transform.GetChild(3).gameObject;
            }
            
            List<GameObject> buttons = new List<GameObject>
            {
                firstButton,
                secondButton,
                thirdButton,
                fourthButton
            };
            
            int randomNumber = Math.Abs(Guid.NewGuid().GetHashCode()) % 4 + 1;

            Sprite randomSprite = _correctAnswerSprite;
            while (randomSprite == _correctAnswerSprite)
            {
                int randomIndex = Random.Range(0, _sprites.Length);
                randomSprite = _sprites[randomIndex];
            }
            
            if (levelToSet < 2)
            {
                // Set the sprite of the correct button
                if (randomNumber <= 2)
                {
                    firstButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                    firstButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

                    secondButton.GetComponent<Image>().sprite = randomSprite;
                    secondButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                }
                else
                {
                    secondButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                    secondButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);

                    firstButton.GetComponent<Image>().sprite = randomSprite;
                    firstButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                }
            }
            else
            {
                List<Sprite> otherSprites = new List<Sprite>(_sprites);
                otherSprites.Remove(_correctAnswerSprite);
                
                foreach (GameObject button in buttons)
                {
                    // Randomly select a sprite from the list
                    int randomIndex = Random.Range(0, otherSprites.Count);
                    randomSprite = otherSprites[randomIndex];

                    // Remove the selected sprite from the list
                    otherSprites.RemoveAt(randomIndex);

                    // Assign the sprite to the button
                    button.GetComponent<Image>().sprite = randomSprite;
                    button.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                }

                switch (randomNumber)
                {
                    case 1:
                        firstButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                        firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
                        firstButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                        break;
                    case 2:
                        secondButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                        secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
                        secondButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                        break;
                    case 3:
                        thirdButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                        thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
                        thirdButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                        break;
                    case 4:
                        fourthButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                        fourthButton.GetComponent<Button>().onClick.RemoveAllListeners();
                        fourthButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                        break;
                    default:
                        Debug.LogError("Random number is not between 1 and 4.");
                        break;
                }
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
