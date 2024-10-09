using UnityEngine;
using TMPro;

public class RhythmGameUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;


    public void UpdateScoreText(string newValue)
    {
        scoreText.text = newValue;
    }

    public void UpdateComboText(string newValue)
    {
        comboText.text = newValue;
    }
}