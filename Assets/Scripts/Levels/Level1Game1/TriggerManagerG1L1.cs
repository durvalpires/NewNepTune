using UnityEngine;

namespace Levels.Level1Game1
{
    public class TriggerManagerG1L1 : MonoBehaviour
    {
        [SerializeField] private bool shouldPressed;
        [SerializeField] private bool shouldHold;
        private bool _havePressed = false;
        public float totalScore;
        

        void Start()
        {
            shouldPressed = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("NoteColliderPress"))
            {
                shouldPressed = true;
            }
            else
            {
                shouldHold = false;
            }
            
            Debug.Log("Girdi");
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Çıktı");
            _havePressed = false;
            shouldPressed = false;
            shouldHold = false;
        }
        
        public bool DoNotaControl()
        {
            if (_havePressed) return false;

            if(shouldPressed || shouldHold)
            {
                Debug.Log("Pressed correctly");
                _havePressed = true;
                
                return true;
            }
            else
            {
                Debug.Log("Pressed incorrectly");
                return false;
            }
        }
    }
}
