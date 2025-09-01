using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualPianoController : MonoBehaviour
{
    [SerializeField] private UnityEvent<string, bool> onPianoKeyTriggered;
    [SerializeField] private VirtualPianoKeyController[] mainPianoKeys;
    [SerializeField] private VirtualPianoKeyController[] blackPianoKeys;
    [SerializeField] private RhythmGameSettings rhythmGameSettings;
    [SerializeField] private PianoAudio pianoAudio;

    private void Start()
    {
        foreach (var key in mainPianoKeys)
        {
            key.SetKeyColor(rhythmGameSettings.ColorSettings.
                GetNoteColor(key.GetNote())); // new Color(0.5f, 0.5f, 0.5f, 1.0f); // <>
        }

        foreach (var blackKey in blackPianoKeys)
        {
            blackKey.SetKeyColor(rhythmGameSettings.ColorSettings.
                GetNoteColor(blackKey.GetNote())); // new Color(0.5f, 0.5f, 0.5f, 1.0f); // <>
        }
    }

    public void EnableKeys(List<string> keys)
    {
        foreach (var key in mainPianoKeys)
            if (keys.Contains(key.GetNote()))
                key.GetComponent<Button>().interactable = true;

        foreach (var blackKey in blackPianoKeys)
            if (keys.Contains(blackKey.GetNote()))
                blackKey.GetComponent<Button>().interactable = true;
    }

//     #region KeyboardInput
// #if UNITY_EDITOR || UNITY_STANDALONE
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.S))
//         {
//             OnCKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.D))
//         {
//             OnDKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.F))
//         {
//             OnEKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.G))
//         {
//             OnFKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.H))
//         {
//             OnGKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.J))
//         {
//             OnAKeyDown();
//         }
//         if (Input.GetKeyDown(KeyCode.K))
//         {
//             OnBKeyDown();
//         }
//
//         if (Input.GetKeyUp(KeyCode.S))
//         {
//             OnCKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.D))
//         {
//             OnDKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.F))
//         {
//             OnEKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.G))
//         {
//             OnFKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.H))
//         {
//             OnGKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.J))
//         {
//             OnAKeyUp();
//         }
//         if (Input.GetKeyUp(KeyCode.K))
//         {
//             OnBKeyUp();
//         }
//     }
//#endif
    //#endregion

    #region Key Events
    
    #region UI Path
    public void OnKeyDown(BaseEventData eventData)
    {
        if (eventData.selectedObject == null) return;
        var pianoKeyController = eventData.selectedObject.GetComponent<VirtualPianoKeyController>();
        if (!pianoKeyController) return;
        Debug.Log("KeyDown: " + pianoKeyController.GetNote());
        onPianoKeyTriggered?.Invoke(pianoKeyController.GetNote(), true);
    }

    public void OnKeyUp(BaseEventData eventData)
    {
        if (eventData.selectedObject == null) return;
        var pianoKeyController = eventData.selectedObject.GetComponent<VirtualPianoKeyController>();
        if (!pianoKeyController) return;
        Debug.Log("KeyUp: " + pianoKeyController.GetNote());
        onPianoKeyTriggered?.Invoke(pianoKeyController.GetNote(), false);
    }
    #endregion

    #region Programmatic API
    public void TriggerKey(string step, bool isDown)
    {
        var k = FindKey(step);
        if (k == null)
        {
            Debug.LogWarning($"[VirtualPianoController] No key found for '{step}'.");
            return;
        }

        Debug.Log((isDown ? "KeyDown (code): " : "KeyUp (code): ") + step);
        onPianoKeyTriggered?.Invoke(step, isDown);

        var btn = k.GetComponent<Button>();
        if (btn && btn.image)
        {
            var colors = btn.colors;
            btn.image.color = isDown ? Color.green : colors.normalColor;
        }
    }

    // public void OnCKeyDown()
    // {
    //     Debug.Log("OnCKeyDown");
    //     onPianoKeyTriggered?.Invoke("C", true);
    // }
    //
    // public void OnCSharpKeyDown()
    // {
    //     Debug.Log("OnCKeyDown");
    //     onPianoKeyTriggered?.Invoke("C", true);
    // }
    //
    // public void OnDKeyDown()
    // {
    //     Debug.Log("OnDKeyDown");
    //     onPianoKeyTriggered?.Invoke("D", true);
    // }
    //
    // public void OnDSharpKeyDown()
    // {
    //     Debug.Log("OnDKeyDown");
    //     onPianoKeyTriggered?.Invoke("D", true);
    // }
    //
    // public void OnEKeyDown()
    // {
    //     Debug.Log("OnEKeyDown");
    //     onPianoKeyTriggered?.Invoke("E", true);
    // }
    //
    // public void OnFKeyDown()
    // {
    //     Debug.Log("OnFKeyDown");
    //     onPianoKeyTriggered?.Invoke("F", true);
    // }
    //
    // public void OnFKeyDown(BaseEventData eventData)
    // {
    //     eventData.
    //     Debug.Log("OnFKeyDown");
    //     onPianoKeyTriggered?.Invoke("F", true);
    // }
    //
    // public void OnGKeyDown()
    // {
    //     Debug.Log("OnGKeyDown");
    //     onPianoKeyTriggered?.Invoke("G", true);
    // }
    //
    // public void OnAKeyDown()
    // {
    //     Debug.Log("OnAKeyDown");
    //     onPianoKeyTriggered?.Invoke("A", true);
    // }
    //
    // public void OnBKeyDown()
    // {
    //     Debug.Log("OnBKeyDown");
    //     onPianoKeyTriggered?.Invoke("B", true);
    // }
    //
    // public void OnCKeyUp()
    // {
    //     Debug.Log("OnCKeyUp");
    //     onPianoKeyTriggered?.Invoke("C", false);
    // }
    //
    // public void OnCSharpKeyUp()
    // {
    //     Debug.Log("OnCKeyUp");
    //     onPianoKeyTriggered?.Invoke("C", false);
    // }
    //
    // public void OnDKeyUp()
    // {
    //     Debug.Log("OnDKeyUp");
    //     onPianoKeyTriggered?.Invoke("D", false);
    // }
    //
    // public void OnDSharpKeyUp()
    // {
    //     Debug.Log("OnDKeyUp");
    //     onPianoKeyTriggered?.Invoke("D", false);
    // }
    //
    // public void OnEKeyUp()
    // {
    //     Debug.Log("OnEKeyUp");
    //     onPianoKeyTriggered?.Invoke("E", false);
    // }
    //
    // public void OnFKeyUp()
    // {
    //     Debug.Log("OnFKeyUp");
    //     onPianoKeyTriggered?.Invoke("F", false);
    // }
    //
    // public void OnFSharpKeyUp()
    // {
    //     Debug.Log("OnFKeyUp");
    //     onPianoKeyTriggered?.Invoke("F", false);
    // }
    //
    // public void OnGKeyUp()
    // {
    //     Debug.Log("OnGKeyUp");
    //     onPianoKeyTriggered?.Invoke("G", false);
    // }
    //
    // public void OnGSharpKeyUp()
    // {
    //     Debug.Log("OnGKeyUp");
    //     onPianoKeyTriggered?.Invoke("G", false);
    // }
    //
    // public void OnAKeyUp()
    // {
    //     Debug.Log("OnAKeyUp");
    //     onPianoKeyTriggered?.Invoke("A", false);
    // }
    //
    // public void OnGSharpKeyUp()
    // {
    //     Debug.Log("OnAKeyUp");
    //     onPianoKeyTriggered?.Invoke("A", false);
    // }
    //
    // public void OnBKeyUp()
    // {
    //     Debug.Log("OnBKeyUp");
    //     onPianoKeyTriggered?.Invoke("B", false);
    // }
    #endregion
    public void PressKey(string step) => TriggerKey(step, true);
    public void ReleaseKey(string step) => TriggerKey(step, false);

    private VirtualPianoKeyController FindKey(string step)
    {
        foreach (var k in mainPianoKeys) if (k && k.GetNote() == step) return k;
        foreach (var k in blackPianoKeys) if (k && k.GetNote() == step) return k;
        return null;
    }
    #endregion

    public VirtualPianoKeyController[] GetMainPianoKeys() => mainPianoKeys;
    public VirtualPianoKeyController[] GetBlackPianoKeys() => blackPianoKeys;
    public void SetMainOctave(int mostUsedOctave) => pianoAudio.SetMainOctave(mostUsedOctave);
}