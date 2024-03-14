using UnityEngine;

public class PianoGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject noteSheetPrefab;

    private PianoPlayer _player;
    private NoteSheet _noteSheet;

    void Start()
    {
        SpawnPlayer();
        SpawnNoteSheet();
    }

    void SpawnPlayer()
    {
        GameObject playerObject = Instantiate(playerPrefab);
        _player = playerObject.GetComponent<PianoPlayer>();
    }

    void SpawnNoteSheet()
    {
        GameObject noteSheetObject = Instantiate(noteSheetPrefab);
        _noteSheet = noteSheetObject.GetComponent<NoteSheet>();
    }
}