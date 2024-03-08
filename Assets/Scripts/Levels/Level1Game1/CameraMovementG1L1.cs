using System.Collections;
using UnityEngine;

/*
 * Can't you just do CameraSpeed=bpm*scale?. Scale is simply a variable you tweak
 * until the camera moves at the correct speed in relation to the music of a specific
 * bpm and then you can use that Scale value across all maps. You then only need to
 * set the bpm variable at the start of each map/level.
*/

public class CameraMovementG1L1 : MonoBehaviour
{
    public float songBPM; // Song BPM

    public float speed = 2f; // Camera speed
    private float startX = -13f; // Start X position
    private float endX = 30f; // End X position

    public bool moveBasedOnBPM;
    
    void Start()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        if (moveBasedOnBPM) StartCoroutine(MoveCamera());
    }

    public float CalculateBPS() => songBPM / 60; // Convert BPM to Beats Per Second
    
    void Update()
    {
        if (moveBasedOnBPM) return;
        
        // Move the camera to the right with the specified speed from its current position
        transform.position += Vector3.right * speed * Time.deltaTime;
        
        // Stop the camera when it reaches the end X position
        if (transform.position.x >= endX)
        {
            // Reset the speed or do something else if you want
            speed = 0;
        }
    }
    
    IEnumerator MoveCamera()
    {
        float speedPerSecond = speed * CalculateBPS(); // Calculate speed per second based on BPS

        while (true)
        {
            // Calculate the new position
            var newPosition = transform.position + Vector3.right * speedPerSecond * Time.deltaTime;

            // Move the camera to the new position
            transform.position = newPosition;

            // Stop the camera when it reaches the end position
            if (transform.position.x >= endX)
            {
                yield break;
            }

            yield return null;
        }
    }
}
