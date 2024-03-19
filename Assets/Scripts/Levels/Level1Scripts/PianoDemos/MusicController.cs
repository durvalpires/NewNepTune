using UnityEngine;

using UnityEngine;

public class MusicController : MonoBehaviour
{
    public GameObject gameObjectWithNotes;
    public GameObject player; // The player GameObject
    public AudioClip musicFile;
    public float bpm;
    public PlatformManager platformManager; // The PlatformManager script
    public MusicBall musicBall; // The MusicBall script

    private AudioSource _audioSource;
}

/* void Start()
 {
     _audioSource = gameObject.AddComponent<AudioSource>();
     _audioSource.clip = musicFile;
     _audioSource.Play();

     MoveGameObjectWithNotesOnce();

     // Get the position of the first platform that spawned
     Vector3 firstPlatformPosition = platformManager.GetFirstPlatformPosition();

     // Start the ball at the position of the first platform
     musicBall.StartBall(bpm, firstPlatformPosition);

     // Calculate the player's spawn position to be 1 beat before the first platform
     Vector3 playerSpawnPosition = firstPlatformPosition - Vector3.right * (bpm / 60f);

     // Set the player's position to the calculated spawn position
     player.transform.position = playerSpawnPosition;

     StartPlayerMovement();
 }

 void MoveGameObjectWithNotesOnce()
 {
     float totalDuration = musicFile.length; // Get the total duration of the music in seconds
     float totalDistance = totalDuration * (bpm / 60f); // Calculate the total distance to move

     gameObjectWithNotes.transform.Translate(Vector3.right * totalDistance);
 }

 void StartPlayerMovement()
 {
     // Start moving the player to the right according to the music's BPM and its closest note
     player.GetComponent<PianoPlayer>().StartMoving(bpm);
 }
}
*/