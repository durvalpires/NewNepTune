using System;
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
    }
}
