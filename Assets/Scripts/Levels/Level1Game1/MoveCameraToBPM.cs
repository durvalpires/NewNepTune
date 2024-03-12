using UnityEngine;

namespace Levels.Level1Game1
{
    public class MoveCameraToBPM : MonoBehaviour
    {
        [SerializeField] private float bpm;

        private float bps;
    
        private void Start()
        {
            bps = bpm / 60;
        }

        private void Update()
        {
            transform.position += new Vector3(0, 0, 0f);
        }
    }
}
