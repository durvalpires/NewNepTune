using UnityEngine;

public class BeatsBouncingBall : MonoBehaviour
{
    public float bpm = 120f;          // Beats per minute from your metronome
    public float bounceHeight = 2f;   // The height of the bounce
    public Transform ball;            // The ball to animate
    private float secondsPerBeat;     // Time in seconds for each beat
    private float timeSinceLastBeat;  // Time counter to track animation progress
    private Vector3 initialPosition;  // Starting position of the ball

    private bool isInitialized = false;

    void Start()
    {
        if(ball == null) ball = this.transform;
        initialPosition = ball.position;  // Save the ball's starting position
    }

    public void SetupBall(float bpm, float beatPerBar)
    {
        this.bpm = bpm;
        secondsPerBeat = 60f / bpm;   // Calculate the time per beat
        isInitialized = true;
    }

    void FixedUpdate()
    {
        if(!isInitialized) return;

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

    public void StopBall()
    {
        isInitialized = false;
        timeSinceLastBeat = 0f;
        ball.position = initialPosition;
    }
}