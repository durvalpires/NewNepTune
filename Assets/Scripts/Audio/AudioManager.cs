using System;
using System.Collections;
using Enums;
using Extensions;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        
        public void PlayMusic(SoundList? clipNameEnum)
        {
            string clipName = clipNameEnum.ToString();
            Sound sound = null;

            foreach (var soundclip in musicSounds)
            {
                if (clipName == soundclip.name)
                {
                    sound = soundclip;
                }
            }
            
            if (sound == null)
            {
                Debug.Log("Sfx not found.");
            }
            else
            {
                musicSource.clip = sound.clip;
                musicSource.Play();
            }
        }
        
        public IEnumerator PlayMusic(SoundList? clipNameEnum, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            string clipName = clipNameEnum.ToString();
            Sound sound = null;

            foreach (var soundclip in musicSounds)
            {
                if (clipName == soundclip.name)
                {
                    sound = soundclip;
                }
            }
            
            if (sound == null)
            {
                Debug.Log("Sfx not found.");
            }
            else
            {
                musicSource.clip = sound.clip;
                musicSource.Play();
            }
        }
        
        public IEnumerator WaitForSeconds(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }
        
        public void PlaySFX(SoundList clipNameEnum)
        {
            string clipName = clipNameEnum.ToString();
            Sound sound = null;

            foreach (var soundclip in sfxSounds)
            {
                if (clipName == soundclip.name)
                {
                    sound = soundclip;
                }
            }
            
            if (sound == null)
            {
                Debug.Log("Sfx not found.");
            }
            else
            {
                sfxSource.PlayOneShot(sound.clip);
            }
        }
        
        public IEnumerator PlaySFX(SoundList? clipNameEnum, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            string clipName = clipNameEnum.ToString();
            Sound sound = null;

            foreach (var soundclip in sfxSounds)
            {
                if (clipName == soundclip.name)
                {
                    sound = soundclip;
                }
            }
            
            if (sound == null)
            {
                Debug.Log("Sfx not found.");
            }
            else
            {
                sfxSource.PlayOneShot(sound.clip);
            }
        }
        
        public IEnumerator SoundFadeOut(AudioSource audioSource, float fadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= .1f;
                Debug.Log("Music volume: " + audioSource.volume);
                yield return new WaitForSeconds(fadeTime);
            }
            
            audioSource.Stop();
            audioSource.volume = 1;
        }
    }
}
