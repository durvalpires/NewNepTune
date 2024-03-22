using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Level1Game1
{
    public class TriggerManagerG1L1 : MonoBehaviour
    {
        [SerializeField] private bool shouldPressed;
        [SerializeField] private bool shouldHold;
        private bool _havePressed = false;
        public float totalScore;
        

        public GameObject finalPanel;
        public GameObject star1, star2, star3;
        public ScoreBarSlider _scoreBarSlider;
        
        private PianoNoteGame _pianoNoteGame;

        void Start()
        {
            shouldPressed = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.gameObject.CompareTag("platform1"))
            {
                shouldPressed = true;
            }
            else if (other.gameObject.CompareTag("platform2") || other.gameObject.CompareTag("platform4"))
            {
                shouldHold = true;
            }
            else if (other.gameObject.CompareTag("platform05"))
            {
                shouldPressed = true;
            }
            
            else if (other.gameObject.CompareTag("NoteMinigameFinish"))
            {
                finalPanel.SetActive(true);
                
                AudioManager.Instance.StopMusic();

                Debug.Log(_scoreBarSlider.slider.value);
                float sliderValue = _scoreBarSlider.slider.value;

                if (sliderValue >= 90)
                {
                    star3.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                }
                if (sliderValue >= 60)
                {
                    star2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                }
                if (sliderValue >= 30)
                {
                    star1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
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
