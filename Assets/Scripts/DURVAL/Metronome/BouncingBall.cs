using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    public float bpm = 120f;          // Beats per minute from your metronome
    public float bounceHeight = 2f;   // The height of the bounce
    public Transform ball;            // The ball to animate
    private float secondsPerBeat;     // Time in seconds for each beat
    private float timeSinceLastBeat;  // Time counter to track animation progress
    private Vector3 initialPosition;  // Starting position of the ball

    void Start()
    {
        if(ball == null) ball = this.transform;
        secondsPerBeat = 60f / bpm;   // Calculate the time per beat
        initialPosition = ball.position;  // Save the ball's starting position
    }

    void Update()
    {
        // Update the time since the last beat
        timeSinceLastBeat += Time.deltaTime;

        // Calculate the normalized time within the beat (0 to 1)
        float beatProgress = timeSinceLastBeat / secondsPerBeat;

        // Use a sine wave to animate the ball smoothly between the bounce points
        float bounce = Mathf.Sin(beatProgress * Mathf.PI);  // Sin wave for smooth bounce

        // Apply the bounce to the ball's Y position
        ball.position = new Vector3(initialPosition.x, initialPosition.y + bounce * bounceHeight, initialPosition.z);

        // Reset the time since last beat after the beat is completed
        if (timeSinceLastBeat >= secondsPerBeat)
        {
            timeSinceLastBeat -= secondsPerBeat;
        }
    }
}