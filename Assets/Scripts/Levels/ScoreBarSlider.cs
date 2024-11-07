using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class ScoreBarSlider : MonoBehaviour
    {
        private List<GameObject> _notesInScene = new List<GameObject>();

        private int notesInScene = -1;

        private GameObject _notesParent;
        public Slider slider;

        public Image star1, star2, star3;

        private bool star1Active = false;
        private bool star2Active = false;
        private bool star3Active = false;

        private float incrementPerNote;
        
        void Start()
        {
            // _notesParent = GameObject.Find("Notes");
            // for (int i = 0; i < _notesParent.transform.childCount; i++)
            // {
            //     _notesInScene.Add(_notesParent.transform.GetChild(i).gameObject);
            // }
        }

        public void SetNotesAmount(int amount){
            notesInScene = amount;
            incrementPerNote = CalculateDivision();
        }
    
        public float CalculateDivision()
        {
            int notesCount = notesInScene;
            if (notesCount == 0)
            {
                return 0;
            }
            return 100f / notesCount;
        }

        public void UpdateSliderBasedOnDivision()
        {

            //float divisionResult = CalculateDivision();
            slider.value += incrementPerNote;
            //Debug.LogWarning("Slider value: " + slider.value);
            
            if(!star1Active && slider.value >= 30)
            {
                star1.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                star1.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                star1Active = true;
            }

            if(!star2Active && slider.value >= 60)
            {
                star2.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                star2.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                star2Active = true;
            }

            if(!star3Active && slider.value >= 90)
            {
                star3.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                star3.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                star3Active = true;
            }                        
            // switch (slider.value)
            // {
            //     case 90:
            //         star3.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
            //         break;
            //     case 60:
            //         star2.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
            //         break;
            //     case 30:
            //         star1.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
            //         break;
            // }
        }
    }
}
