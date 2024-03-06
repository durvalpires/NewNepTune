using Audio;
using Enums;
using UnityEngine;

namespace Levels.PianoGame
{
    public class PianoPlay : MonoBehaviour
    {
        public void PlayNote(string note)
        {
            note = note.ToUpper();
            switch (note)
            {
                case "A":
                    AudioManager.Instance.PlaySFX(SoundList.note_A);
                    break;
                case "B":
                    AudioManager.Instance.PlaySFX(SoundList.note_B);
                    break;
                case "C":
                    AudioManager.Instance.PlaySFX(SoundList.note_C);
                    break;
                case "D":
                    AudioManager.Instance.PlaySFX(SoundList.note_D);
                    break;
                case "E":
                    AudioManager.Instance.PlaySFX(SoundList.note_E);
                    break;
                case "F":
                    AudioManager.Instance.PlaySFX(SoundList.note_F);
                    break;
                case "G":
                    AudioManager.Instance.PlaySFX(SoundList.note_G);
                    break;
                default:
                    Debug.Log("Invalid note");
                    break;
            }
            
            Debug.Log("Pressed " + note + " note on Piano.");
        }
    }
}
