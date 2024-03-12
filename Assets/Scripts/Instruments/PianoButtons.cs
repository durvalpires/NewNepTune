using System;
using Audio;
using Enums;
using Levels;
using Levels.Level1Game1;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Instruments
{
    public class PianoButtons : EventTrigger
    {
        private CameraMovementG1L1 _cameraMovementG1L1;
        TriggerManagerG1L1 _triggerManagerG1L1;
        ScoreBarSlider _scoreBarSlider;
        
        private void Start()
        {
            _triggerManagerG1L1 = FindObjectOfType<TriggerManagerG1L1>();
            _scoreBarSlider = FindObjectOfType<ScoreBarSlider>();
            _cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
        }

        public void PressPianoKey(string pressedNote)
        {
            SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());

            if (_triggerManagerG1L1.DoNotaControl()) //Runs if pressed correctly
            {
                _scoreBarSlider.UpdateSliderBasedOnDivision();
                //can add animations here
            }
            
            AudioManager.Instance.sfxSource.volume = 1;
            
            if (_soundFadeOut != null) StopCoroutine(_soundFadeOut);

            AudioManager.Instance.PlaySFX(soundToPlay);
        }
        
        public void NotePressRemoved()
        {
            _soundFadeOut = StartCoroutine(AudioManager.Instance.SoundFadeOut
                (AudioManager.Instance.sfxSource, .1f));
        }

        private Coroutine _soundFadeOut;
    }
}
