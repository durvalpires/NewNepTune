using System.Collections;
using Enums;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public Sound[] musicSounds, sfxSounds, miniGameSounds;
        public AudioSource musicSource, sfxSource, BgMusicSource;


        private void Start()
        {
            HandleSceneMusic(SceneManager.GetActiveScene().name);
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene current, Scene next)
        {
            HandleSceneMusic(next.name);
        }

        private void HandleSceneMusic(string sceneName)
        {
            if (sceneName == "v2_Levels" || sceneName == "v2_SelectionMinigame")
            {
                PlayMusic(SoundList.LevelMenu, true);
            }
            else
            {
                StopMusic(true);
            }
        }


        public void PlayMusic(SoundList? clipNameEnum, bool isBackground = false)
        {
            if (clipNameEnum == null) return;

            Sound sound = System.Array.Find(musicSounds, s => s.name == clipNameEnum.ToString());

            if (sound == null)
            {
                Debug.LogWarning($"Music '{clipNameEnum}' not found.");
                return;
            }

            AudioSource source = isBackground ? BgMusicSource : musicSource;

            if (source.clip == sound.clip && source.isPlaying) return;

            source.clip = sound.clip;
            source.Play();
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

        public void StopMusic(bool isBackground = false)
        {
            AudioSource source = isBackground ? BgMusicSource : musicSource;

            if (source.isPlaying)
            {
                source.Stop();
                source.clip = null;
            }
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

        public void PlaySFX(SoundList sfxType)
        {
            Sound sound = null;

            foreach (var soundclip in sfxSounds)
            {
                if (sfxType == soundclip.type)
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

        public float GetMusicTime()
        {
            return musicSource.time;
        }

        public float SetMusicTime(float time)
        {
            return musicSource.time = time;
        }
    }
}
