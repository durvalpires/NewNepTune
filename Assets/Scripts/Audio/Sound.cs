using Enums;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public SoundList type;
        public AudioClip clip;
    }
}
