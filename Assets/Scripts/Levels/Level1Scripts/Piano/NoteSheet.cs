/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSheet : MonoBehaviour
{
    public GameObject notePrefab;
    public float bpm;

    private List<NoteScript> _notes = new List<NoteScript>();

    void Start()
    {
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        float spawnInterval = 60f / bpm; // Calculate the time between each note spawn

        while (true)
        {
            SpawnNote();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnNote()
    {
        GameObject noteObject = Instantiate(notePrefab, transform);
        NoteScript note = noteObject.GetComponent<NoteScript>();
        _notes.Add(note);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSheet : MonoBehaviour
{
    public GameObject notePrefab;
    public float bpm;
    public float noteSpacing = 1f; // The distance between each note

    private List<NoteScript> _notes = new List<NoteScript>();
    private Vector3 _lastSpawnPosition;

    void Start()
    {
        _lastSpawnPosition = transform.position; // Initialize the last spawn position to the position of the NoteSheet
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        float spawnInterval = 60f / bpm; // Calculate the time between each note spawn

        while (true)
        {
            SpawnNote();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnNote()
    {
        // Calculate the spawn position of the new note
        Vector3 spawnPosition = _lastSpawnPosition + Vector3.right * noteSpacing;

        GameObject noteObject = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
        NoteScript note = noteObject.GetComponent<NoteScript>();
        _notes.Add(note);

        _lastSpawnPosition = spawnPosition; // Update the last spawn position
    }
}
