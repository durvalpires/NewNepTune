using UnityEngine;

public class NoteScript : MonoBehaviour
{
    private BoxCollider2D _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void OnPlayerInteraction()
    {
        // Handle what happens when the player interacts with this note
    }
}