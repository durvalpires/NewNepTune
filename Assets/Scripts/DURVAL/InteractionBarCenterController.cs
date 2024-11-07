using UnityEngine;
using UnityEngine.Events;

public class InteractionBarCenterController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<NoteController> onNoteTriggeredInteractionBarCenter;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        onNoteTriggeredInteractionBarCenter?.Invoke(other.gameObject.GetComponent<NoteController>());
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Note"))
    //     {
    //         // Check if the note is interactable at this point
    //         // Allow player to hit the note
    //
    //         onNoteTriggeredInteractionBarCenter?.Invoke(other.gameObject.GetComponent<NoteController>(), false);
    //     }
    // }
}
