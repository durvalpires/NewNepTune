using System;
using UnityEngine;

namespace DURVAL
{
    public class RestController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer whole;
        [SerializeField] private SpriteRenderer half;
        [SerializeField] private SpriteRenderer quarter;
        [SerializeField] private SpriteRenderer eighth;
        [SerializeField] private SpriteRenderer sixteenth;

        private void Awake()
        {
            whole.color = Color.black;
            half.color = Color.black;
            quarter.color = Color.black;
            eighth.color = Color.black;
            sixteenth.color = Color.black;
        }

        public void SetupRestSprite(string length)
        {
            switch (length.ToLower())
            {
                case "whole":
                    whole.gameObject.SetActive(true);
                    half.gameObject.SetActive(false);
                    quarter.gameObject.SetActive(false);
                    eighth.gameObject.SetActive(false);
                    sixteenth.gameObject.SetActive(false);
                    break;
                case "half":
                    whole.gameObject.SetActive(false);
                    half.gameObject.SetActive(true);
                    quarter.gameObject.SetActive(false);
                    eighth.gameObject.SetActive(false);
                    sixteenth.gameObject.SetActive(false);
                    break;
                case "quarter":
                    whole.gameObject.SetActive(false);
                    half.gameObject.SetActive(false);
                    quarter.gameObject.SetActive(true);
                    eighth.gameObject.SetActive(false);
                    sixteenth.gameObject.SetActive(false);
                    break;
                case "eighth":
                    whole.gameObject.SetActive(false);
                    half.gameObject.SetActive(false);
                    quarter.gameObject.SetActive(false);
                    eighth.gameObject.SetActive(true);
                    sixteenth.gameObject.SetActive(false);
                    break;
                case "sixteenth":
                    whole.gameObject.SetActive(false);
                    half.gameObject.SetActive(false);
                    quarter.gameObject.SetActive(false);
                    eighth.gameObject.SetActive(false);
                    sixteenth.gameObject.SetActive(true);
                    break;
                default:
                    whole.gameObject.SetActive(false);
                    half.gameObject.SetActive(false);
                    quarter.gameObject.SetActive(false);
                    eighth.gameObject.SetActive(false);
                    sixteenth.gameObject.SetActive(false);
                    break;
            }
        }
    }
}