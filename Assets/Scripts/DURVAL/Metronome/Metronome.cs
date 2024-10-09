using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMetronome
{
    public class Metronome : MonoBehaviour
    {
        public delegate void MetronomeEvent(int beatCount);
        public static event MetronomeEvent tic;
        [SerializeField] private float bpm = 120.0f;
        private float beatInterval;   //how long one beat is;
        private float beatTimer;
        private int beatCounter;

        [Header("InputField")]
        [SerializeField] private InputField inputField_bpm;

        private void Start() {
            inputField_bpm.onValueChanged.AddListener(OnInputFieldChanged);
        }
        void Update()
        {
            beatInterval = 60/bpm;

            beatTimer += Time.deltaTime;

            if(beatTimer >= beatInterval){
                beatTimer = 0;
                beatCounter ++;

                tic.Invoke(beatCounter);
            }
        }

        void OnInputFieldChanged(string value)
        {
            if(float.TryParse(value, out float result))
            {
                bpm = result;
            }
        }
    }

}