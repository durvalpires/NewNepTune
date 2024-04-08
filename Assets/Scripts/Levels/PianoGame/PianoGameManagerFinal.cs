using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PianoGameManagerFinal : MonoBehaviour
{
    public GameObject backGroundForCountdown;
    public GameObject countdownCanvas;
    public GameObject pianoGameCanvas;
    public Text countdownText;
    //public ParticleSystem particalStar;
    private void Awake()
    {
        pianoGameCanvas.SetActive(false);
        //particalStar.Stop();
    }

    void Start()
    {
        backGroundForCountdown.SetActive(true);
        countdownCanvas.SetActive(true);
        StartCoroutine(Countdown());
        //particalStar.Stop();
    }

    // Update is called once per frame
   
    
    public void StartGame()
    {
        backGroundForCountdown.SetActive(false);
        countdownCanvas.SetActive(false);
        pianoGameCanvas.SetActive(true);
        
    }
    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        StartGame();
    }
    
    public IEnumerator ShowStar()
    {
       // particalStar.Play();
        yield return new WaitForSeconds(1);
        //particalStar.Stop();
    }
    
}
