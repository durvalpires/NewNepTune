using System;
using System.Collections;
using UnityEngine;

public class NoteBasedBouncingBall : MonoBehaviour
{
    private bool levelStarted;
    private float targetY = -100;
    private float noteHitYOffset = .3f;
    private float bounceDuration;
    private float bounceOffset = .5f;
    private float timeElapsedSinceBounce;
    private float initialY;

    private void Start()
    {
        StartCoroutine(BouncingCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator BouncingCoroutine()
    {
        while (true)
        {
            if (levelStarted || targetY != -100)
            {
                // Increase the timer based on time passed since the last frame
                timeElapsedSinceBounce += Time.deltaTime;

                // Calculate the normalized time (0 to 1) over the bounce duration
                float normalizedTime = timeElapsedSinceBounce / bounceDuration;

                if (normalizedTime <= 1f) // Animate only while within the bounce duration
                {
                    // Calculate the parabolic bounce height using sine wave
                    float yOffset = Mathf.Sin(normalizedTime * Mathf.PI) * bounceOffset;

                    // Calculate the current Y position using interpolation and offset
                    float currentY = Mathf.Lerp(initialY, targetY, normalizedTime) + yOffset;

                    // Apply the calculated position to the GameObject
                    transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
                }
            }
            yield return null;
        }
    }
    
    public void OnLevelStarted(){
        levelStarted = true;}
    

    public void OnNextNoteUpdated(NoteView noteView, float currentBeat, float secPerBeat, float beatsPerUnit)
    {
        initialY = transform.position.y;
        var noteSpriteBounds = noteView.GameObject.GetComponent<NoteController>().CircleSprite.bounds;
        var noteColliderHalfXSize = noteView.GameObject.GetComponent<BoxCollider2D>().size.x / 2;
        var ballSpriteBounds = GetComponent<SpriteRenderer>().bounds;
        targetY = noteSpriteBounds.center.y + noteSpriteBounds.extents.y
            + ballSpriteBounds.extents.y / 2f;

        bounceDuration = Mathf.Abs(secPerBeat * ((noteView.beatNumber-currentBeat)));
        timeElapsedSinceBounce = 0;

    }
}
