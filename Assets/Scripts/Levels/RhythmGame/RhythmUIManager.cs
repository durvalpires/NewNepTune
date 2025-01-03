using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using TMPro;
using RtMidi;


public class RhythmUIManager : MonoBehaviour
{
   
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI feedbackText;

    private StringBuilder feedbackBuilder = new StringBuilder();


    public IEnumerator CountdownRoutine(int seconds, string messageBefore = "")
    {
        while (seconds > 0)
        {
            countdownText.text = $"{messageBefore}\n{seconds}";
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
    }

    public void SetFeedbackText(string text)
    {
        if (feedbackText != null)
        {
            feedbackText.text = text;
        }
    }

    public void ClearFeedback()
    {
        feedbackBuilder.Clear();
        if (feedbackText != null) feedbackText.text = "";
    }

    public void AppendFeedback(string line)
    {
        feedbackBuilder.AppendLine(line);
        if (feedbackText != null) feedbackText.text = feedbackBuilder.ToString();
    }
  
}
