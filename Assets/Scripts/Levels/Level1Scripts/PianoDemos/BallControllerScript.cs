using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallControllerScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
 
    public Transform _groundY;
    public Transform _maxHeight;
    private float _speed;
    private float songBPS;
    
    private List<GameObject> notes = new List<GameObject>();
    public GameObject notesParent;
    
    private int currentNodeIndex = 0;
    public GameObject ball;
    private CameraMovementG1L1 cameraMovementG1L1;

    void Start()
    {
        cameraMovementG1L1 = FindObjectOfType<CameraMovementG1L1>();
        foreach (Transform child in notesParent.transform)
        {
            notes.Add(child.gameObject);
        }
        songBPS = cameraMovementG1L1.GetSongBPS();
        StartCoroutine(MoveBall());
    }
    
   /* private IEnumerator MoveBall()
    {
        while (currentNodeIndex < notes.Count - 1)
        {
            GameObject currentNote = notes[currentNodeIndex];
            GameObject nextNote = notes[currentNodeIndex + 1];

            // Calculate the distance between the current platform and the next platform
            float distance = Vector3.Distance(currentNote.transform.position, nextNote.transform.position);

            // Use this distance to adjust the speed of the ball's movement
            float speed = distance / (_maxHeight.position.y - _groundY.position.y);

            // Start moving the ball up
            while (transform.position.y < _maxHeight.position.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _maxHeight.position.y, transform.position.z), speed * Time.deltaTime);
                yield return null;
            }

            // Start moving the ball down
            while (transform.position.y > _groundY.position.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _groundY.position.y, transform.position.z), speed * Time.deltaTime);
                yield return null;
            }

            currentNodeIndex++;
        }
    }*/
   private IEnumerator MoveBall()
   {
       while (currentNodeIndex < notes.Count - 1)
       {
           GameObject currentNote = notes[currentNodeIndex];
           GameObject nextNote = notes[currentNodeIndex + 1];

           // Calculate the distance between the current platform and the next platform
           float distance = Vector3.Distance(currentNote.transform.position, nextNote.transform.position);

           // Use the tag of the colliding gameobject to adjust the speed of the ball's movement
           float speed;
           if (currentNote.CompareTag("platform2"))
           {
               speed = distance / 2f;
           }
           else if (currentNote.CompareTag("platform05"))
           {
               speed = distance;
           }
           else
           {
               speed = distance / 1.5f;
           }

           // Adjust the height of the jump based on the speed and distance
           _maxHeight.position = new Vector3(_maxHeight.position.x, _groundY.position.y + speed, _maxHeight.position.z);

           // Calculate the time it takes for the ball to move up and down
           float time = distance / speed;

           // Start moving the ball up
           float elapsedTime = 0;
           while (elapsedTime < time / 2)
           {
               transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _maxHeight.position.y, transform.position.z), speed * Time.deltaTime);
               elapsedTime += Time.deltaTime;
               yield return null;
           }

           // Start moving the ball down
           elapsedTime = 0;
           while (elapsedTime < time / 2)
           {
               transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, nextNote.transform.position.y, transform.position.z), speed * Time.deltaTime);
               elapsedTime += Time.deltaTime;
               yield return null;
           }

           currentNodeIndex++;
       }
   }
}