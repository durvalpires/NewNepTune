using System.Collections;
using Enums;
using Extensions;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public Sound[] musicSounds, sfxSounds, miniGameSounds;
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

        public virtual IEnumerator PlayMusic(SoundList? clipNameEnum, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            PlayMusic(clipNameEnum);
        }
        
        public void PauseMusic()
        {
            musicSource.Pause();
        }
        
        public void UnPauseMusic()
        {
            musicSource.UnPause();
        }
        
        public void StopMusic()
        {
            musicSource.clip = null;
            musicSource.Stop();
        }
        
        public bool CheckIfMusicIsPlaying()
        {
            return musicSource.isPlaying;
        }

        public void ChangeMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }
        
        public void ChangeSFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }
        
        public IEnumerator WaitForSeconds(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }
        
        public void PlaySFX(SoundList? clipNameEnum)
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
        
        public float CheckSoundLength(SoundList? clipNameEnum)
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
                return 0;
            }
            else
            {
                return sound.clip.length;
            }
        }
        
        public IEnumerator PlaySFX(SoundList? clipNameEnum, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            PlaySFX(clipNameEnum);
        }
        
        public IEnumerator SoundFadeOut(AudioSource audioSource, float fadeTime)
        {
            float startVolume = audioSource.volume;
            
            while (audioSource.volume > 0)
            {
                audioSource.volume -= .2f; 
                yield return new WaitForSeconds(fadeTime);
            }
            
            audioSource.Stop();
            audioSource.volume = 1;
        }
        
        public void PlayMinigameMusic(SoundList? clipNameEnum)
        {
            string clipName = clipNameEnum.ToString();
            Sound sound = null;

            foreach (var soundclip in miniGameSounds)
            {
                if (clipName == soundclip.name)
                {
                    sound = soundclip;
                }
            }
            
            if (sound == null)
            {
                Debug.Log("Minigame music not found.");
            }
            else
            {
                musicSource.clip = sound.clip;
                musicSource.Play();
            }
        }
    }
}
