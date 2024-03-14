using System;
using UnityEngine;

public class BallControllerScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float bpm;
    private float _jumpHeight;
    private float _speed;

    public GameObject ParentNotes;
    
    public GameObject[] platforms;
    private int _currentPlatformIndex = 0;

    void Start()
    {
        _jumpHeight = bpm / 60f; // Adjust this value as needed
        _speed = bpm / 60f; // Adjust this value as needed
        FallToFirstNote();
    }

    void AdjustJumpHeight(string platformTag)
    {
        // Adjust the jump height based on the platform's tag
        switch (platformTag)
        {
            case "Tag1":
                _jumpHeight = bpm / 60f; // Adjust this value as needed
                break;
            case "Tag2":
                _jumpHeight = bpm / 30f; // Adjust this value as needed
                break;
            // Add more cases as needed
        }
    }
    void FallToFirstNote()
    {
        // Assuming the first note is the first platform
        transform.position = platforms[0].transform.position + Vector3.up * Vector3.Distance(transform.position, platforms[_currentPlatformIndex].transform.position); // Start from above the first platform
        _rigidbody.velocity = Vector3.down * 10; // Fall down
    }

    void Jump()
    {
        // Make the ball jump
        _rigidbody.AddForce(new Vector3(0, _jumpHeight, 0), (ForceMode2D)ForceMode.Impulse);
    }

    void Update()
    {
        // Get all the child GameObjects of the parent GameObject
        GameObject[] childGameObjects = new GameObject[ParentNotes.transform.childCount];
        for (int i = 0; i < ParentNotes.transform.childCount; i++)
        {
            childGameObjects[i] = ParentNotes.transform.GetChild(i).gameObject;
        }

        // Find the closest child GameObject to the ball
        GameObject closestChild = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject child in childGameObjects)
        {
            float distance = Vector3.Distance(transform.position, child.transform.position);
            if (distance < closestDistance && distance > 0.1f) // Skip the current platform that the ball is on
            {
                closestDistance = distance;
                closestChild = child;
            }
        }

        // Move the ball towards the closest child GameObject
        if (closestChild != null)
        {
            Vector3 direction = (closestChild.transform.position - transform.position).normalized;
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Platform platform = other.gameObject.GetComponent<Platform>();
        if (platform != null)
        {
            AdjustJumpHeight(other.gameObject.tag);
            Jump();
        }
    }
}