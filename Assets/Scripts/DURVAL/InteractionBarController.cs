using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionBarController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<string, bool> onNoteTriggeredInteractionBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            // Check if the note is interactable at this point
            // Allow player to hit the note

            onNoteTriggeredInteractionBar?.Invoke(other.gameObject.GetComponent<NoteController>().Pitch.Step, true);
            Debug.LogWarning("OnTriggerEnter2D: " + other.gameObject.GetComponent<NoteController>().Pitch.Step);
        }
    }

    void OnTrigger(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            // Check if the note is interactable at this point
            // Allow player to hit the note

            onNoteTriggeredInteractionBar?.Invoke(other.gameObject.GetComponent<NoteController>().Pitch.Step, false);
            Debug.LogWarning("OnTriggerExit2D: " + other.gameObject.GetComponent<NoteController>().Pitch.Step);
        }
    }

}
