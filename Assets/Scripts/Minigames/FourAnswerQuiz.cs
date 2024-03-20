using System.Collections.Generic;
using Audio;
using Enums;
using UnityEngine;

public class FourAnswerQuiz : MonoBehaviour
{
    public List<Question> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject finishPanel;

    public GameObject[] gameLevels;
    public AudioClip[] audioClips;
    private AudioSource _audioSource;
    
    private int _currentLevel;

    void Start()
    {
        for (int i = 0; i < gameLevels.Length; i++)
        {
            //SetUpLevel(i);
            gameLevels[i].gameObject.SetActive(false);
        }
         
        gameLevels[0].gameObject.SetActive(true);

        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = audioClips[_currentLevel];
      
        _audioSource.Play();
        
        CurrentQuestionIndex = 0;
    }

    public void TrueAnswer()
    {
        _audioSource.Stop();
        winPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SoundList.WinSound);
        gameLevels[_currentLevel].gameObject.SetActive(false);
    }

    public void FalseAnswer()
    {
        _audioSource.Stop();
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
public class Answer
{
    public string Text { get; set; }
    public Sprite Image { get; set; }

    public Answer(string text, Sprite image)
    {
        Text = text;
        Image = image;
    }
}

public class Question
{
    public string Text { get; set; }
    public List<Answer> Answers { get; set; }

    public Question(string text, List<Answer> answers)
    {
        Text = text;
        Answers = answers;
    }
}
