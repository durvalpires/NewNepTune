using System;
using System.Collections;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Level1Game1
{
    public class TriggerManagerG1L1 : MonoBehaviour
    {
        [SerializeField] private bool shouldPressed;
        [SerializeField] private bool shouldHold;
        private bool _havePressed = false;
        private bool _haveCleared = false;
        public float totalScore;
        private CameraMovementG1L1 _cameraMovementG1L1;
        private SpriteRenderer _playerSpriteREnderer;
        private VirtualPianoGameStateManager _virtualPianoGameStateManager;
        public bool shouldStopGame = false;

        public GameObject finalPanel;
        public GameObject star1, star2, star3;
        public ScoreBarSlider _scoreBarSlider;

        private PianoNoteGame _pianoNoteGame;

        public string _pressedNote;
        private NoteData _noteData;
        private string _noteTypeToPlay;
        private PianoGameManagerFinal _pianoGameManagerFinal;

        private AudioSource _audioSource;
        public float _audioSourceTime;

        private Vector3 _lastNotePosition;
        private PianoManag _pianoManager;

        void Start()
        {
            shouldPressed = false;
            _cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
            _pianoGameManagerFinal = FindObjectOfType<PianoGameManagerFinal>();
            _playerSpriteREnderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
            _audioSource = FindObjectOfType<AudioSource>();
            _virtualPianoGameStateManager = FindObjectOfType<VirtualPianoGameStateManager>();
            _pianoManager = FindObjectOfType<PianoManag>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _noteData = other.GetComponent<NoteData>();
            if (_noteData != null)
            {
                
                SetNoteTypeToPlay(_noteData.NoteType);
                Debug.Log("Note Type: " + _noteTypeToPlay);
                if (other.gameObject.CompareTag("platform1"))
                {
                    _cameraMovementG1L1.UpdateLastTriggeredPosition();
                    _pianoManager.SaveAudioTime();
                    shouldPressed = true;
                    _playerSpriteREnderer.color = new Color(1f, 0.92f, 0.016f, 0.5f);
                }
                else if (other.gameObject.CompareTag("platform2") || other.gameObject.CompareTag("platform4"))
                {
                    _cameraMovementG1L1.UpdateLastTriggeredPosition();
                    _pianoManager.SaveAudioTime();
                    shouldHold = true;
                    _playerSpriteREnderer.color = new Color(1f, 0.92f, 0.016f, 0.5f);
                }
                else if (other.gameObject.CompareTag("platform05"))
                {
                    _cameraMovementG1L1.UpdateLastTriggeredPosition();
                    _pianoManager.SaveAudioTime();
                    shouldPressed = true;
                    _playerSpriteREnderer.color = new Color(1f, 0.92f, 0.016f, 0.5f);
                }


                else if (other.gameObject.CompareTag("NoteMinigameFinish"))
                {
                    finalPanel.SetActive(true);

                    AudioManager.Instance.StopMusic();

                    Debug.Log(_scoreBarSlider.slider.value);
                    float sliderValue = _scoreBarSlider.slider.value;

                    if (sliderValue >= 90)
                    {
                        star3.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    }

                    if (sliderValue >= 60)
                    {
                        star2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    }

                    if (sliderValue >= 30)
                    {
                        star1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    }
                    else if (_havePressed)
                    {
                        shouldStopGame = true;
                        _virtualPianoGameStateManager.StartFalseAnswerSolution();
                    }
                }

            }
        }
        

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_havePressed)
            {
                shouldStopGame = true;
            }
            _haveCleared = false;
            _havePressed = false;
            shouldPressed = false;
            shouldHold = false;
            _playerSpriteREnderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public bool DoNotaControl()
        {
            if (_haveCleared)
            {
                return false;
            }

            if (_havePressed)
            {
                
                _havePressed = true;
                shouldStopGame = true;
                _virtualPianoGameStateManager.StartFalseAnswerSolution();
                return false;
            }

            if (shouldPressed || shouldHold)
            {
                if (_pressedNote == _noteTypeToPlay)
                {
                    Debug.Log("Pressed correctly");
                    _haveCleared = true;
                    _havePressed = true;
                    StartCoroutine(returnPlayerOriginalColorTrue());
                    shouldStopGame = false;
                    return true;
                }
                else
                {
                    if (shouldStopGame)
                    {
                        shouldStopGame = true;
                        return false;
                    }
                    _havePressed = true;
                    shouldStopGame = true;
                    _virtualPianoGameStateManager.StartFalseAnswerSolution();
                    StartCoroutine(returnPlayerOriginalColorFalse());
                    return false;
                }
            }
            if ((shouldPressed || shouldHold) && !_havePressed)
            {
                shouldStopGame = true;
                _virtualPianoGameStateManager.StartFalseAnswerSolution();
                return false;
            }
            else
            {
                _havePressed = true;
               // shouldStopGame = true;
               // _virtualPianoGameStateManager.StartFalseAnswerSolution();
                Debug.Log("Pressed incorrectly");
                StartCoroutine(returnPlayerOriginalColorFalse());
                return false;
            }

        }
    

    public void SetPressedNote(string note)
        {
            _pressedNote = note;
        }

        public void SetNoteTypeToPlay(string noteType)
        {
            _noteTypeToPlay = noteType;
        }

        private IEnumerator returnPlayerOriginalColorTrue()
        {
            _playerSpriteREnderer.color = new Color(0f, 1f, 0f, 0.5f);
            yield return new WaitForSeconds(1);

            _playerSpriteREnderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            yield break;
        }

        private IEnumerator returnPlayerOriginalColorFalse()
        {
            _playerSpriteREnderer.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(1);

            _playerSpriteREnderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            yield break;
        }

      
    }
}
