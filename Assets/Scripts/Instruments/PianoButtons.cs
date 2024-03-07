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
        TriggerManagerG1L1 _triggerManagerG1L1;
        ScoreBarSlider _scoreBarSlider;
        
        private void Start()
        {
            _triggerManagerG1L1 = FindObjectOfType<TriggerManagerG1L1>();
            _scoreBarSlider = FindObjectOfType<ScoreBarSlider>();
        }

        public override void OnPointerClick(PointerEventData data) //Pressed once
        {
        }

        public override void OnPointerDown(PointerEventData data) //Started pressing down
        {
            if (_triggerManagerG1L1.DoNotaControl())
            {
                _scoreBarSlider.UpdateSliderBasedOnDivision();
            }
            
            AudioManager.Instance.sfxSource.Stop();
            AudioManager.Instance.sfxSource.volume = 1;
            
            if (_soundFadeOut != null) StopCoroutine(_soundFadeOut);
            
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.sfxSounds[2].clip; 
            //TODO : This above is hardcoded, it should be changed to a more dynamic way.
            AudioManager.Instance.sfxSource.Play();
        }

        private Coroutine _soundFadeOut;
        
        public override void OnPointerUp(PointerEventData data) //Stopped pressing down
        {
            _soundFadeOut = StartCoroutine(AudioManager.Instance.SoundFadeOut(AudioManager.Instance.sfxSource, .1f));
        }
    }
}
