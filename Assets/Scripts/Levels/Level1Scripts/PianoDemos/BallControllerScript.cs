using System;
using Unity.VisualScripting;
using UnityEngine;

public class BallControllerScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float bpm;
    private float _groundY;
    private float _maxHeight;
    private float _speed;

    public GameObject ball;
  private CameraMovementG1L1 cameraMovementG1L1;

  void Start()
  {
      _rigidbody = GetComponent<Rigidbody2D>();
      _rigidbody.gravityScale = 0; // Enable gravity
      
  }

    
 private void OnTriggerEnter2D (Collider2D other)
  {
      if (other.gameObject.CompareTag("platform1") || other.gameObject.CompareTag("platform2"))
      {
          ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector3(0, -1, 0), 2f);
      }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
        if (other.gameObject.CompareTag("platform1") || other.gameObject.CompareTag("platform2"))
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector3(0, -1, 0), 2f);
        }
  }
}