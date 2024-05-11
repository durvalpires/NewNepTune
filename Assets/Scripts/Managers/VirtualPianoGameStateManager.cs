using System.Collections;
using System.Collections.Generic;
using Levels.Level1Game1;
using UnityEngine;

public class VirtualPianoGameStateManager : MonoBehaviour
{
    private CameraMovementG1L1 _cameraMovementG1L1;
    private TriggerManagerG1L1 _triggerManagerG1L1;
    private AudioSource _audioSource;
    private PianoGameManagerFinal _pianoGameManagerFinal;
    
    private bool isGameStopped = false;
    
    
    void Start()
    {
        _audioSource = FindObjectOfType<AudioSource>();
        _cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
        _triggerManagerG1L1 = FindObjectOfType<TriggerManagerG1L1>();
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
            _audioSource.Stop();
            _cameraMovementG1L1.ReturnToLastTriggeredPosition();
            _cameraMovementG1L1.StopAllCoroutines();
            _audioSource.time = _triggerManagerG1L1._audioSourceTime;
    }

    public void ResumeGame()
    {
        isGameStopped = false;
        _audioSource.Play();
        _cameraMovementG1L1.StartCoroutine(_cameraMovementG1L1.MoveCamera());
    }
}
