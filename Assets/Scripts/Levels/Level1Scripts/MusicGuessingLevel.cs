using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

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
               for (int i = 1; i < gameLevels.Length; i++)
               {
                   gameLevels[i].gameObject.SetActive(false);
               }

               AudioManager.Instance.PlayMinigameMusic(SoundList.FirstSoundMG);
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
   
           public void nextLevelButton()
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
