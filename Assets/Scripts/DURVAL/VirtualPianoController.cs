using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualPianoController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<string, bool> onPianoKeyTriggered;

    [SerializeField]
    private Button[] mainPianoKeys;

    [SerializeField]
    private RhythmGameSettings rhythmGameSettings;

    private void Start()
    {
        foreach (Button button in mainPianoKeys)
        {
            var pianoKeyController = button.GetComponent<VirtualPianoKeyController>();
            pianoKeyController.SetKeyColor(rhythmGameSettings.ColorSettings.
                GetNoteColor(pianoKeyController.GetNote())); // new Color(0.5f, 0.5f, 0.5f, 1.0f); // <>
        }
    }




    public void OnCKeyDown()
    {
        Debug.Log("OnCKeyDown");
        onPianoKeyTriggered?.Invoke("C", true);
    }

    public void OnDKeyDown()
    {
        Debug.Log("OnDKeyDown");
        onPianoKeyTriggered?.Invoke("D", true);
    }

    public void OnEKeyDown()
    {
        Debug.Log("OnEKeyDown");
        onPianoKeyTriggered?.Invoke("E", true);
    }

    public void OnFKeyDown()
    {
        Debug.Log("OnFKeyDown");
        onPianoKeyTriggered?.Invoke("F", true);
    }

    public void OnGKeyDown()
    {
        Debug.Log("OnGKeyDown");
        onPianoKeyTriggered?.Invoke("G", true);
    }

    public void OnAKeyDown()
    {
        Debug.Log("OnAKeyDown");
        onPianoKeyTriggered?.Invoke("A", true);
    }

    public void OnBKeyDown()
    {
        Debug.Log("OnBKeyDown");
        onPianoKeyTriggered?.Invoke("B", true);
    }

    public void OnCKeyUp()
    {
        Debug.Log("OnCKeyUp");
        onPianoKeyTriggered?.Invoke("C", false);
    }

    public void OnDKeyUp()
    {
        Debug.Log("OnDKeyUp");
        onPianoKeyTriggered?.Invoke("D", false);
    }

    public void OnEKeyUp()
    {
        Debug.Log("OnEKeyUp");
        onPianoKeyTriggered?.Invoke("E", false);
    }

    public void OnFKeyUp()
    {
        Debug.Log("OnFKeyUp");
        onPianoKeyTriggered?.Invoke("F", false);
    }

    public void OnGKeyUp()
    {
        Debug.Log("OnGKeyUp");
        onPianoKeyTriggered?.Invoke("G", false);
    }

    public void OnAKeyUp()
    {
        Debug.Log("OnAKeyUp");
        onPianoKeyTriggered?.Invoke("A", false);
    }

    public void OnBKeyUp()
    {
        Debug.Log("OnBKeyUp");
        onPianoKeyTriggered?.Invoke("B", false);
    }

}
