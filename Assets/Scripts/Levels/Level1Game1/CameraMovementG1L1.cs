using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Can't you just do CameraSpeed=bpm*scale?. Scale is simply a variable you tweak
 * until the camera moves at the correct speed in relation to the music of a specific
 * bpm and then you can use that Scale value across all maps. You then only need to
 * set the bpm variable at the start of each map/level.
*/

public class CameraMovementG1L1 : MonoBehaviour
{
    [SerializeField] private float songBPM; // Song BPM
    private float songBPS => songBPM / 60;

    public GameObject notesParent;
    public int notCount;

    private List<GameObject> notes = new List<GameObject>();
    
    [SerializeField] private float startX = -13f; // Start X position
    [SerializeField] private float endX = 76.98f; // End X position
    // set this positions based on first note and finishLineCollider

    private void Start()
    {
        foreach (Transform child in notesParent.transform)
        {
            notes.Add(child.gameObject);
        }

        // Sort the notes based on their x position
        notes.Sort((note1, note2) => note1.transform.position.x.CompareTo(note2.transform.position.x));

        StartCoroutine(MoveCamera());
    }

    public IEnumerator MoveCamera()
    {
        int currentNoteIndex = 0;
        float beatDuration = 1 / songBPS; // Default beat duration

        while (currentNoteIndex < notes.Count)
        {
            GameObject currentNote = notes[currentNoteIndex];
            notCount = currentNoteIndex;
            // Calculate the start position and end position of the movement
            Vector3 startPosition = transform.position;
            Vector3 endPosition = new Vector3(currentNote.transform.position.x, transform.position.y, transform.position.z);

            // Calculate the start time and end time of the movement
            float startTime = Time.time;
            float endTime = startTime + beatDuration;

            // Move the camera to the current note's position over the duration of one beat
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / beatDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            // Move on to the next note
            currentNoteIndex++;

            // Check the tag of the current note and adjust the beat duration for the next movement
            switch (currentNote.tag)
            {
                    case "platform2":
                    beatDuration = 2 / songBPS;
                    break;
                case "platform4":
                    beatDuration = 4 / songBPS;
                    break;
                case "platform05":
                    beatDuration = 0.5f / songBPS;
                    break;
                case "shush":
                    beatDuration = 1 / songBPS;
                    break;
                default:
                    beatDuration = 1 / songBPS;
                    break;
            }
        }
    }
    public float GetSongBPS()
    {
        return songBPS;
    }
    
}
