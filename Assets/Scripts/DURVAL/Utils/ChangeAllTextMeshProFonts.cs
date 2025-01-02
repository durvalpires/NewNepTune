using UnityEngine;
using TMPro;

public class ChangeAllTextMeshProFonts : MonoBehaviour
{
    [Header("Set the TMP Font Asset to apply to all TextMeshPro components")]
    public TMP_FontAsset newFontAsset;

    [Header("Optional: Apply at startup")]
    public bool applyAtStart = false;

    void Start()
    {
        if (applyAtStart && newFontAsset != null)
        {
            ChangeAllTMPFonts();
        }
    }

    [ContextMenu("Change All TextMeshPro Fonts")]
    public void ChangeAllTMPFonts()
    {
        if (newFontAsset == null)
        {
            Debug.LogError("No TMP Font Asset assigned! Please assign a TMP Font Asset in the inspector.");
            return;
        }

        // Find all TextMeshPro and TextMeshProUGUI components in the project
        TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>(true); // true to include inactive GameObjects
        Debug.Log($"Found {allTMPTexts.Length} TextMeshPro components in the project.");

        foreach (TMP_Text tmpText in allTMPTexts)
        {
            tmpText.font = newFontAsset;
        }

        Debug.Log("All TextMeshPro fonts have been updated.");
    }
}