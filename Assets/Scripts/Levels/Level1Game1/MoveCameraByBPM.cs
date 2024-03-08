using UnityEngine;

public class CameraMotionByBPM : MonoBehaviour
{
    public float songBPM; // Song BPM
    public float baseSpeed = 1f; // Base speed
    private float startX = -13f; // Start X position
    private float endX = 30f; // End X position

    void Start()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Calculate the speed based on the song's BPM
        float calculatedSpeed = CalculateSpeedBasedOnBPM();

        // Move the camera to the right with the calculated speed from its current position
        transform.position += Vector3.right * calculatedSpeed * Time.deltaTime;

        // Stop the camera when it reaches the end X position
        if (transform.position.x >= endX)
        {
            // Reset the speed or do something else if you want
            calculatedSpeed = 0;
        }
    }

    float CalculateSpeedBasedOnBPM()
    {
        // Convert BPM to BPS (Beats Per Second)
        float bps = songBPM / 60;

        // Multiply the base speed by the BPS to get the speed based on the song's BPM
        return baseSpeed * bps;
    }
}