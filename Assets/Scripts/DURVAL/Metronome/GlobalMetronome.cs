using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuantizedLoopStation
{
    public class GlobalMetronome : MonoBehaviour
    {
        public delegate void GlobalMetronomeEvent(int count);
        public static event GlobalMetronomeEvent tic;
        public static event GlobalMetronomeEvent subTic;
        public static GlobalMetronome instance;

        [SerializeField] private bool playSubTic = false;
        [SerializeField] private bool playTic = false;

        private float bpm = 80.0f;
        private float beatInvertal;
        public float BeatInvertal => beatInvertal;
        private float beatTimer;
        private int beatCount;

        private int numberBeatInBar = 4;
        public int NumberBeatInBar => numberBeatInBar;
        private int quantizeDegree = 2;
        public int QuantizeDegree => quantizeDegree;


        private float subBeatTimer;
        private int subBeatCount;


        private AudioSource audioSource;
        public AudioClip[] metronomeClip;

        private bool isInitialized = false;

        [SerializeField] private RhythmGameSettings rhythmGameSettings;
        public UnityEvent OnPreCountdownDone;
        private bool preCountdownDone = false;
        public UnityEvent OnTwoBeatsLeft;
        private bool twoBeatsLeftDone = false;
        public UnityEvent OnBeatTap;
        public UnityEvent OnBackgroundTrackTrigger;

        private int backgroundTrackIntroBeats = 2;


        //[SerializeField] private LoopStation curLoopStation;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }

            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
        }

        private void OnEnable()
        {
            tic += OnTic;
            subTic += OnSubTic;
        }

        private void OnDisable()
        {
            tic -= OnTic;
            subTic -= OnSubTic;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if(!isInitialized) return;
            
            beatInvertal = 60f / bpm;

            subBeatTimer += Time.fixedDeltaTime;
            if (subBeatTimer >= beatInvertal / quantizeDegree)
            {
                subBeatTimer -= beatInvertal / quantizeDegree;
                subBeatCount++;

                if (subBeatCount % quantizeDegree == 0)
                {
                    // beatCount++;
                    // if(beatCount < rhythmGameSettings.beatsBeforeStart - this.numberBeatInBar + 1 
                    //    || beatCount > rhythmGameSettings.beatsBeforeStart) 
                    //     return;
                    //
                    // tic(beatCount);
                    if (subBeatCount % quantizeDegree == 0)
                    {
                        beatCount++;

                        int firstTicBeat = rhythmGameSettings.beatsBeforeStart - numberBeatInBar + 1;
                        if (beatCount >= firstTicBeat && beatCount <= rhythmGameSettings.beatsBeforeStart)
                        {
                            tic?.Invoke(beatCount);
                        }
                    }
                }

                if (playSubTic) subTic(subBeatCount);
            }
            
            if (beatCount == rhythmGameSettings.beatsBeforeStart - backgroundTrackIntroBeats)
            {
                OnBackgroundTrackTrigger?.Invoke();
            }

            if (!twoBeatsLeftDone && beatCount == rhythmGameSettings.beatsBeforeStart - 1)
            {
                Debug.Log("Two beats left " + beatCount);
                OnTwoBeatsLeft.Invoke();
                twoBeatsLeftDone = true;
            }

            if(!preCountdownDone && beatCount == rhythmGameSettings.beatsBeforeStart+1)
            {
                Debug.Log("Pre countdown done " + beatCount);
                OnPreCountdownDone?.Invoke();
                preCountdownDone = true;
                StopMetronome();
            }
        }

        public void StopMetronome()
        {
            isInitialized = false;
        }

        // float deltaTime;
        // private void Update()
        // {
        //     deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //     float fps = 1.0f / deltaTime;

        //     Debug.Log(fps);
        // }

        public int GetCurrentQuantizedSubBeat()
        {
            if (subBeatTimer <= beatInvertal / quantizeDegree / 2f)
            {
                return subBeatCount;
            }
            else
            {
                return subBeatCount + 1;
            }
        }

        // WHERE EVERYTHING IS KICKSTARTED
        public void SetupMetronome(float bpm, int beatPerBar, int backTrackIntroBeats)
        {
            Debug.Log("Setting up metronome");
            Debug.Log("BPM: " + bpm);
            Debug.Log("Beat per bar: " + beatPerBar);
            Debug.Log("Background track intro beats: " + backTrackIntroBeats);
            this.bpm = bpm;
            this.numberBeatInBar = beatPerBar;
            this.backgroundTrackIntroBeats = backTrackIntroBeats;
            isInitialized = true;
        }

        // public void AddLoopNote(int _instrumentID, int _noteID)
        // {
        //     if (!curLoopStation.IsNoteAddable()) return;
        //     curLoopStation.AddLoopNote(GetCurrentQuantizedSubBeat(), _instrumentID, _noteID);
        // }

        private void OnTic(int beat)
        {
            audioSource.PlayOneShot(metronomeClip[0], 1.0f);
        }

        private void OnSubTic(int subBeat)
        {
            if(playSubTic) 
                audioSource.PlayOneShot(metronomeClip[0], 0.1f);
        }
    }
}


