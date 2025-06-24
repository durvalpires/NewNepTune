using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        //protected Sprite[] _sprites;
        //private Sprite _correctAnswerSprite;
        
        protected GameObject[] _prefabs;
        private GameObject _correctAnswerPrefab;
        [FormerlySerializedAs("correctAnswerSpriteName")] public string correctAnswerGOName;
        public string levelToReturn;
        protected string contentType;
        
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject finishedBackButton;


       
        [SerializeField] private AllGameScoringConfig gameScoringConfig;

        private SelectionScoringSettings _selectionSettings;
        private int _score;
        private int _pointsPerQuestion;


        private int _currentLevel;

        private void Start()
        {
            _selectionSettings = gameScoringConfig.selectionScoring;
            _pointsPerQuestion = _selectionSettings.maxScore;
            _score = 0;

            //Sprite[] answerSprites = Resources.LoadAll<Sprite>($"SelectionMinigame/Notes/");
            _correctAnswerPrefab = Array.Find(_prefabs, go => go.name == correctAnswerGOName.ToUpper());
            
            backButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(levelToReturn);
            });
            finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(levelToReturn);
            });
            
            levelText.text = $"Which one is {correctAnswerGOName}?";
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

            List<GameObject> GOs = new List<GameObject>();
            
            int randomNumber = Math.Abs(Guid.NewGuid().GetHashCode()) % 4 ;

            // Sprite randomSprite = _correctAnswerSprite;
            // while (randomSprite == _correctAnswerSprite)
            // {
            //     int randomIndex = Random.Range(0, _sprites.Length);
            //     randomSprite = _sprites[randomIndex];
            // }
            
            GameObject randomPrefab = _correctAnswerPrefab;
            while (randomPrefab == _correctAnswerPrefab)
            {
                int randomIndex = Random.Range(0, _prefabs.Length);
                randomPrefab = _prefabs[randomIndex];
            }
            
            if (levelToSet < 2)
            {
                // Set the sprite of the correct button
                // if (randomNumber <= 2)
                // {
                //     firstButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //     firstButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //
                //     secondButton.GetComponent<Image>().sprite = randomSprite;
                //     secondButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                // }
                // else
                // {
                //     secondButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //     secondButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //
                //     firstButton.GetComponent<Image>().sprite = randomSprite;
                //     firstButton.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                // }
                // Set the prefab of the correct button
                if (randomNumber < 2)
                {
                    var go = MakePrefabInteractable(firstButton, _correctAnswerPrefab);
                    go.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                    
                    GOs.Add(go);

                    go = MakePrefabInteractable(secondButton, randomPrefab);
                    go.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                    
                    GOs.Add(go);
                }
                else
                {
                    var go = MakePrefabInteractable(secondButton, _correctAnswerPrefab);
                    go.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                    
                    GOs.Add(go);

                    go =MakePrefabInteractable(firstButton, randomPrefab);
                    go.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                    
                    GOs.Add(go);
                }
            }
            else
            {
                List<GameObject> otherGOs = new List<GameObject>(_prefabs);
                otherGOs.Remove(_correctAnswerPrefab);
                
                foreach (GameObject button in buttons)
                {
                    // Randomly select a sprite from the list
                    int randomIndex = Random.Range(0, otherGOs.Count);
                    var randomGO = otherGOs[randomIndex];

                    // Remove the selected sprite from the list
                    otherGOs.RemoveAt(randomIndex);
                    
                    var go = MakePrefabInteractable(button, randomGO);
                    go.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                    
                    GOs.Add(go);

                    // // Assign the sprite to the button
                    // button.GetComponent<Image>().sprite = randomSprite;
                    // button.GetComponent<Button>().onClick.AddListener(FalseAnswer);
                }

                GOs.RemoveAt(randomNumber);
                var correctGO = MakePrefabInteractable(buttons[randomNumber], _correctAnswerPrefab);
                correctGO.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                GOs.Insert(randomNumber, correctGO);

                // switch (randomNumber)
                // {
                //     case 1:
                //         GOs.RemoveAt(randomNumber);
                //         firstButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //         firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
                //         firstButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //         break;
                //     case 2:
                //         secondButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //         secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
                //         secondButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //         break;
                //     case 3:
                //         thirdButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //         thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
                //         thirdButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //         break;
                //     case 4:
                //         fourthButton.GetComponent<Image>().sprite = _correctAnswerSprite;
                //         fourthButton.GetComponent<Button>().onClick.RemoveAllListeners();
                //         fourthButton.GetComponent<Button>().onClick.AddListener(TrueAnswer);
                //         break;
                //     default:
                //         Debug.LogError("Random number is not between 1 and 4.");
                //         break;
                // }
            }
        }

        public void TrueAnswer()
        {
            Debug.Log(_score);
            winPanel.SetActive(true);
            AudioManager.Instance.PlaySFX(SoundList.WinSound);
            gameLevels[_currentLevel].gameObject.SetActive(false);
        }

        public void FalseAnswer()
        {
            _score -= _selectionSettings.wrongAnswerPenalty;
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

                float normalized = (float)_score / _selectionSettings.maxScore;
                int stars = 0;
                if (normalized >= _selectionSettings.threeStarThreshold) stars = 3;
                else if (normalized >= _selectionSettings.twoStarThreshold) stars = 2;
                else if (normalized >= _selectionSettings.oneStarThreshold) stars = 1;
                _score = _score + _pointsPerQuestion;
                int safeScore = _score < 0 ? 0 : _score;
                PlayerModelBase.SetCustomScore(safeScore);
                Debug.Log(safeScore);
                PlayerModelBase.LevelDataService.SetCustomScore(safeScore);
                PlayerModelBase.LevelDataService.SetCustomStarRating(stars);

                finishPanel.SetActive(true);
            }
        }
        
        public void Restart()
        {
            losePanel.SetActive(false);
            gameLevels[_currentLevel].gameObject.SetActive(true);
        }
        
        // Helper method to instantiate a prefab and add a button component
        private GameObject MakePrefabInteractable(GameObject button, GameObject prefab)
        {
            GameObject instantiatedPrefab = Instantiate(prefab, button.transform);
            //button.GetComponent<Button>().targetGraphic = instantiatedPrefab.GetComponent<Image>();
            var img = instantiatedPrefab.GetComponent<Image>();
            img.raycastTarget = true;
            instantiatedPrefab.AddComponent<Button>().targetGraphic = img;
            
            // Add a RectTransform component to the instantiated prefab
            var rectTransform = instantiatedPrefab.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                rectTransform = instantiatedPrefab.AddComponent<RectTransform>();
            }

            // Set the instantiated prefab to stretch to its parent's limits
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
            
            return instantiatedPrefab;
        }
    }
}
