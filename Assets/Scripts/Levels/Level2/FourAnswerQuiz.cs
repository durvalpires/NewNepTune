using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FourAnswerQuiz : MonoBehaviour
{
    public List<Question> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }

    // UI elements
    public Text QuestionText;
    public List<Button> AnswerButtons;
    public List<Image> AnswerImages;

    void Start()
    {
        // Initialize your questions and answers here
        Questions = new List<Question>
        {
            new Question("Question 1", new List<Answer>
            {
                new Answer("Answer 1", AnswerImages[0].sprite),
                new Answer("Answer 2" ,AnswerImages[1].sprite),
                new Answer("Answer 3" ,AnswerImages[2].sprite),
                new Answer("Answer 4", AnswerImages[3].sprite)
            }),
            // Add more questions as needed
        };

        CurrentQuestionIndex = 0;
        DisplayCurrentQuestion();
    }

    void DisplayCurrentQuestion()
    {
        Question currentQuestion = Questions[CurrentQuestionIndex];
        QuestionText.text = currentQuestion.Text;

        for (int i = 0; i < AnswerButtons.Count; i++)
        {
            AnswerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.Answers[i].Text;
            AnswerImages[i].sprite = currentQuestion.Answers[i].Image;
        }
    }

    public void OnAnswerButtonClicked(int answerIndex)
    {
        // Check if the selected answer is correct
        // If correct, move on to the next question
        // If incorrect, display a message or do something else
    }
}
public class Answer
{
    public string Text { get; set; }
    public Sprite Image { get; set; }

    public Answer(string text, Sprite image)
    {
        Text = text;
        Image = image;
    }
}

public class Question
{
    public string Text { get; set; }
    public List<Answer> Answers { get; set; }

    public Question(string text, List<Answer> answers)
    {
        Text = text;
        Answers = answers;
    }
}
