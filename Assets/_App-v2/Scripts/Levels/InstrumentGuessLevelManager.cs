using Minigames;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstrumentGuessLevelManager : InstrumentGuess
{
    private void Awake()
    {
        var data = TempDataStorage.GetSceneData<InstrumentGuessLevelSO>();
        if (data) levelInstrument = data.instrument.ToLower();
        var spritesList = Resources.LoadAll<Sprite>($"InstrumentPNGs/").ToList();
        spritesList.Shuffle();
        _sprites = spritesList.ToArray();
        Debug.LogWarning(_sprites[0]);
    }

    private void InitCorrectAnswers()
    {
         _correctAnswerSprites = new Sprite[gameLevels.Length];
         _correctAudioClips = new AudioClip[gameLevels.Length];
         int answerIndex = 0;

         for (int i = 0; i < _sprites.Length; i++)
         {
            Debug.LogWarning(_sprites[i].name);
            if (_sprites[i].name.ToLower() == levelInstrument)
            {
                Debug.Log("Found right answer");
                if (answerIndex < _correctAnswerSprites.Length)
                    _correctAnswerSprites[answerIndex++] = _sprites[i];
            }
         }

        Debug.LogWarning(_sprites.Length);

         for (int i = 0; i < _correctAnswerSprites.Length; i++)
         {
            Debug.Log(_correctAnswerSprites[0].name);
            _correctAudioClips[i] = Resources.Load<AudioClip>($"InstrumentSounds/{_correctAnswerSprites[i].name}");
         }
    }

    protected override void Start()
    {
        InitCorrectAnswers();
        base.Start();
        character.GetComponent<Animator>().Play($"BoyAst{_correctAnswerSprites[_currentLevel].name}");
          
        if (levelToReturn == "") Debug.LogError("Level to return is not set!");

        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(levelToReturn);
        });
        finishedBackButton.onClick.AddListener(() =>
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
        _audioSource.clip = _correctAudioClips[_currentLevel];
         
        StartCoroutine(PlayAfterSeconds(.8f));
    }
}
