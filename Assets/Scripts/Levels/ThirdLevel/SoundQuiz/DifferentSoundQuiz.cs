using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifferentSoundQuiz : MonoBehaviour
{
    public GameObject[] levels;
   
    public AudioSource audioSource;
    
    public GameObject falseCanvas;
    public AudioClip[] audioClips;
    
    private int _currentLevel;
    
    
    public void correctAnswer()
    {
        if (_currentLevel + 1 != levels.Length)
        {
            levels[_currentLevel].SetActive(false);

            _currentLevel++;
            levels[_currentLevel].SetActive(true);
        }
    }

    public void falseAnswer()
    {
        falseCanvas.SetActive(true);
        levels[_currentLevel].SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    
    public void startButtonClicked()
    {
        int currentSound = _currentLevel / 2;
        audioSource.clip = audioClips[currentSound];
        audioSource.Play();
    }
}
