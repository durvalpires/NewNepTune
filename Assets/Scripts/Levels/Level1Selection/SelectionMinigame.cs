using System;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Levels.Level1Selection
{
    public class SelectionMinigame : MonoBehaviour
    {
        public GameObject scenePanel;
        public GameObject winPanel;
        public GameObject losePanel;
        public GameObject finishPanel;

        public GameObject[] gameLevels;

        private int _currentLevel;

        private void Start()
        {
            for (int i = 1; i < gameLevels.Length; i++)
            {
                gameLevels[i].gameObject.SetActive(false);
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

        public void nextLevelButton()
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
