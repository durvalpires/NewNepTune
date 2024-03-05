using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizGameManager : MonoBehaviour
{
    QuizManager quiz;
    EndGameManager endScreen;

    void Awake() //unity manual - execution order araştırılmalı. awake kullanmamızın sebebi bu
    {
        quiz = FindObjectOfType<QuizManager>();
        endScreen = FindObjectOfType<EndGameManager>();
    }
    void Start()
    {
        quiz.gameObject.SetActive(true); //quiz ekranı açık kalsın başlangıçta
        endScreen.gameObject.SetActive(false); //bitiş ekranı kapalı kalsın başlangıçta
    }

    // Update is called once per frame
    void Update()
    {
        if(quiz.isQuizComplete) //eğer quiz bittiyse
        {
            quiz.gameObject.SetActive(false); //quiz ekranını kapat
            endScreen.gameObject.SetActive(true); //bitiş ekranını aç
            endScreen.ShowFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //aktif sahneyi tekrar başlatır bu kod.
    }
}
