using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
   /* public GameObject movingObject;
    public List<GameObject> platforms;
    public float bpm;
    public PianoManag pianoManag;

    public int currentPlatformIndex = 0;
    private float speed;
    public AudioSource music;

    private GameObject _currentPlatform;

    private float timeUntilNextBeat;
    public GameObject nextPlatform;
    private float distanceToNextPlatform;
    private float baseSpeed;

    void Start()
    {
        baseSpeed = bpm / 60f;
        speed = baseSpeed;
        _currentPlatform = platforms[currentPlatformIndex];
        if (music != null)
        {
            music.Play();
        }
    }
      void Update()
      {
          if (music.isPlaying && currentPlatformIndex < platforms.Count)
          {
              _currentPlatform = platforms[currentPlatformIndex];
               nextPlatform = platforms[currentPlatformIndex + 1];
              // Calculate the time until the next beat
              timeUntilNextBeat = 60f / bpm;

              // Calculate the distance to the next platform
               distanceToNextPlatform = Vector3.Distance(movingObject.transform.position, nextPlatform.transform.position);

              // Adjust the speed so that the moving object reaches the next platform in time for the next beat
              float calculatedSpeed = distanceToNextPlatform / timeUntilNextBeat;

              // Calculate the base speed based on the bpm
             

              float step = speed * Time.deltaTime;

            
              if (movingObject.transform.position == nextPlatform.transform.position)
              {
                  currentPlatformIndex++;
                  pianoManag.HighLightPlatform();
              }

              if (_currentPlatform.CompareTag("platform1") && nextPlatform.CompareTag("platform1") && distanceToNextPlatform > speed)
              {
                  speed = calculatedSpeed;
              }
              else
              {
                  speed = baseSpeed;
              }
              movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, nextPlatform.transform.position, step);
          }
      } 

    /*void Update()
    {
        _currentPlatform = platforms[currentPlatformIndex];
        if (music.isPlaying && currentPlatformIndex < platforms.Count - 1)
        {
            nextPlatform = platforms[currentPlatformIndex + 1];
            float baseSpeed = bpm / 60f;

            distanceToNextPlatform = Vector3.Distance(movingObject.transform.position, nextPlatform.transform.position);
            float step = speed * Time.deltaTime;
            movingObject.transform.position =
                Vector3.MoveTowards(movingObject.transform.position, nextPlatform.transform.position, step);

            if (_currentPlatform.CompareTag("platform1") && nextPlatform.CompareTag("platform1") &&
                distanceToNextPlatform > timeUntilNextBeat)
            {

                float calculatedSpeed = distanceToNextPlatform / timeUntilNextBeat;

                // Use the calculated speed if it's higher than the base speed
                speed = calculatedSpeed;
            }
            else
            {
                speed = baseSpeed;
            }

            if (movingObject.transform.position == nextPlatform.transform.position)
            {
                currentPlatformIndex++;
                pianoManag.HighLightPlatform();
            }


        }
    }*/
}   
    

