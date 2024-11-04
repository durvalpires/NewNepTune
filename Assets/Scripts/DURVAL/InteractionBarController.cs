using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractionBarController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<NoteController, bool> onNoteTriggeredInteractionBar;

    [SerializeField] private RhythmGameSettings settings;
    [FormerlySerializedAs("collider")] [SerializeField] private BoxCollider2D mainCollider;

    // Start is called before the first frame update
    void Start()
    {
        mainCollider.size = new Vector2(mainCollider.size.x * (int)settings.difficulty, mainCollider.size.y);
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

            onNoteTriggeredInteractionBar?.Invoke(other.gameObject.GetComponent<NoteController>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            // Check if the note is interactable at this point
            // Allow player to hit the note

            onNoteTriggeredInteractionBar?.Invoke(other.gameObject.GetComponent<NoteController>(), false);
        }
    }

}
