using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundQuizLevelManager : MonoBehaviour
{
    public GameObject infoCanvas;
    public GameObject imageBlocker;
    public GameObject falseAnswerCanvas;
    
    public Text questionText;

    public Button[] answerButtons;

    private string[] questions = { "what is this", "what is this 2" };
    private string[] correctAnswers = { "violin", "piano" };
    private string[] falseAnswers = { "violin", "piano" };
    
    
    public AudioSource audioSource;
    public AudioClip firstQuestion;
    public AudioClip secondQuestion;

    private bool _quizAnswer;
    
    public void LevelStart()
    {
        infoCanvas.SetActive(false);
        imageBlocker.SetActive(true);
        StartQuiz();
    }

    public void StartQuiz()
    {
        int randomIndex = Random.Range(0, questions.Length);
        string question = questions[randomIndex];
        string correctAnswer = correctAnswers[randomIndex];

        // Set the question text
        questionText.text = question;

        // Shuffle the answers
        ShuffleAnswers();

        // Assign the answers to the buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i == 0)
                answerButtons[i].GetComponentInChildren<Text>().text = correctAnswer;
            else
                answerButtons[i].GetComponentInChildren<Text>().text = falseAnswers[randomIndex];
        }
        StartCoroutine(FirstTest());
        
        
    }
    void ShuffleAnswers()
    {
        // Fisher-Yates shuffle algorithm
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int randomIndex = Random.Range(i, answerButtons.Length);
            Button temp = answerButtons[i];
            answerButtons[i] = answerButtons[randomIndex];
            answerButtons[randomIndex] = temp;
        }
    }
    public void OnAnswerButtonClick(Button button)
    {
        string buttonText = button.GetComponentInChildren<Text>().text;
        if (buttonText == correctAnswers[0] || buttonText == correctAnswers[1])
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Incorrect!");
        }

        // After answering, initialize the quiz again for a new question
        StartQuiz();
    }

    private void Update()
    {
        if (_quizAnswer == true)
        {
            imageBlocker.SetActive(false);
        }
        else
        {
            falseAnswerCanvas.SetActive(true);
        }
    }
    
    IEnumerator FirstTest()
     {
         yield return new WaitForSeconds(2);
         audioSource.clip = firstQuestion;
         audioSource.Play();
     }
    

    IEnumerator SecondTest()
    {
        yield return new WaitForSeconds(2);
        audioSource.clip = secondQuestion;
        audioSource.Play();
    }
    
   
}
