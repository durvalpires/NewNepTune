using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreManager scoreKeeper;
    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreManager>();
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Congratulations!\nYou got a score of " + 
        scoreKeeper.CalculateScore() + "%";
    }
}
