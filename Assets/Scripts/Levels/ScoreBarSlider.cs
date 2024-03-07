using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Levels
{
    public class ScoreBarSlider : MonoBehaviour
    {
        private List<GameObject> _notesInScene = new List<GameObject>();
        private GameObject _notesParent;
        public Slider slider;

        public Image star1, star2, star3;
        
        void Start()
        {
            _notesParent = GameObject.Find("Notes");
            for (int i = 0; i < _notesParent.transform.childCount; i++)
            {
                _notesInScene.Add(_notesParent.transform.GetChild(i).gameObject);
            }
        }
    
        public float CalculateDivision()
        {
            int notesCount = _notesInScene.Count;
            if (notesCount == 0)
            {
                return 0;
            }
            return 100f / notesCount;
        }

        public void UpdateSliderBasedOnDivision()
        {
            float divisionResult = CalculateDivision();
            slider.value += divisionResult;

            switch (slider.value)
            {
                case 90:
                    star3.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    break;
                case 60:
                    star2.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    break;
                case 30:
                    star1.sprite = Resources.Load<Sprite>("Sprites/star-fulled");
                    break;
            }
        }
    }
}
