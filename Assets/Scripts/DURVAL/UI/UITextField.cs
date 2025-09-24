using TMPro;
using UnityEngine;

public class UITextField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;

    public void SetText(string value)
    {
        if (textElement != null)
            textElement.text = value;
    }
}