using System;
using System.Linq;
using Minigames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicGuessLevelManager : MusicGuessingLevel
{
    private void Awake()
    {
        var data = TempDataStorage.GetSceneData<MusicGuessLevelSO>();
        if(data)levelNote = data.levelNote;
        if(data) galaxy = data.galaxy.ToString();
        var spritesList = Resources.LoadAll<Sprite>($"MusicGuess/Sprites/{galaxy}/{levelNote}/").ToList();
        spritesList.Shuffle();
        _sprites = spritesList.ToArray();
        InitCorrectAnswers();
    }

    private void InitCorrectAnswers()
    {
        //assign shuffled sprites
        correctAnswerSprites = new Sprite[3];
        correctAnswerSprites[0] = _sprites[0];
        correctAnswerSprites[1] = _sprites[1];
        correctAnswerSprites[2] = _sprites[2];
        audioClips = new AudioClip[3];
        audioClips[0] = Resources.Load<AudioClip>($"MusicGuess/Sounds/{galaxy}/{levelNote}/{correctAnswerSprites[0].name}");
        audioClips[1] = Resources.Load<AudioClip>($"MusicGuess/Sounds/{galaxy}/{levelNote}/{correctAnswerSprites[1].name}");
        audioClips[2] = Resources.Load<AudioClip>($"MusicGuess/Sounds/{galaxy}/{levelNote}/{correctAnswerSprites[2].name}");
    }
    protected override void Start()
    {
        _character = GameObject.Find("karakter");
        _character.GetComponent<Animator>().Play("RedGirlPiano");
         
         
        if (levelToReturn == "") Debug.LogError("Level to return is not set!");

        backButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(levelToReturn);
        });
        finishedBackButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(levelToReturn);
        });
         
        
        
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
}
