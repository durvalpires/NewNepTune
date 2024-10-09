using System.Collections;
using System.Collections.Generic;
using Audio;
using Levels.Level1Game1;
using UnityEngine;

public class VirtualPianoGameStateManager : MonoBehaviour
{
    private CameraMovementG1L1 _cameraMovementG1L1;
    private TriggerManagerG1L1 _triggerManagerG1L1;
    private AudioSource _audioSource;
    private PianoGameManagerFinal _pianoGameManagerFinal;
    
    private PianoManag _pianoManager;
    private bool isGameStopped = false;
    
    
    void Start()
    {
        // _audioSource = FindObjectOfType<AudioSource>();
        //Debug.Log("AudioSource - " + _audioSource.gameObject.name);
        _cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
        _triggerManagerG1L1 = FindObjectOfType<TriggerManagerG1L1>();
        _pianoManager = FindObjectOfType<PianoManag>();
    }

    
    void Update()
    {
        if (_triggerManagerG1L1.shouldStopGame && !isGameStopped)
        {
            StartFalseAnswerSolution();
        }
        else if (!_triggerManagerG1L1.shouldStopGame && isGameStopped)
        {
            ResumeGame();
        }
    }

    public void StartFalseAnswerSolution()
    {
            isGameStopped = true;
            _pianoManager.StopAudio();
            _cameraMovementG1L1.ReturnToLastTriggeredPosition();
            _cameraMovementG1L1.StopAllCoroutines();
            //AudioManager.Instance.SetMusicTime(_triggerManagerG1L1._audioSourceTime);
    }

    public void ResumeGame()
    {
        isGameStopped = false;
        _pianoManager.StartAudioByTime();
        _cameraMovementG1L1.StartCoroutine(_cameraMovementG1L1.MoveCamera());
    }
}
