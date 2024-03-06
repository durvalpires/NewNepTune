using Audio;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Instruments
{
    public class PianoButtons : EventTrigger
    {
        public override void OnPointerClick(PointerEventData data) //Pressed once
        {
            Debug.Log("OnPointerClick called.");
        }

        public override void OnPointerDown(PointerEventData data) //Started pressing down
        {
            AudioManager.Instance.sfxSource.Stop();
            AudioManager.Instance.sfxSource.volume = 1;
            
            if (_soundFadeOut != null) StopCoroutine(_soundFadeOut);
            
            Debug.Log("OnPointerDown called.");
            AudioManager.Instance.sfxSource.clip = AudioManager.Instance.sfxSounds[2].clip; 
            //TODO : This above is hardcoded, it should be changed to a more dynamic way.
            AudioManager.Instance.sfxSource.Play();
        }

        private Coroutine _soundFadeOut;
        
        public override void OnPointerUp(PointerEventData data) //Stopped pressing down
        {
            Debug.Log("OnPointerUp called.");
            _soundFadeOut = StartCoroutine(AudioManager.Instance.SoundFadeOut(AudioManager.Instance.sfxSource, .1f));
        }
    }
}
