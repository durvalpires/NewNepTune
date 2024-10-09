using System.Collections;
using UnityEngine;

public class PianoPlayer : MonoBehaviour
{
    public float bpm;
    public GameObject[] platforms; // Array of platforms
    private Rigidbody2D _rigidbody;
    private int _currentPlatformIndex = 0;

    public GameObject gameManager;
    private PlatformManager _platformManager;

    IEnumerator _waitForClickCoroutine;

   private void Start()
    {
        _platformManager = gameManager.GetComponent<PlatformManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        FallToFirstNote();
    }

   private void FallToFirstNote()
    {
        // Assuming the first note is the first platform
        transform.position = platforms[0].transform.position + Vector3.up *
            Vector3.Distance(transform.position,
                platforms[_currentPlatformIndex].transform.position); // Start from above the first platform
        _rigidbody.linearVelocity = Vector3.down * 10; // Fall down
    }

   private void JumpToNextPlatform()
    {
        if (_currentPlatformIndex < platforms.Length - 1)
        {
            _currentPlatformIndex++;
            Vector3 direction = (platforms[_currentPlatformIndex].transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, platforms[_currentPlatformIndex].transform.position);
            float jumpSpeed = (bpm / 60f) * distance; // Adjust speed according to bpm and distance
            PlatformType platformType = platforms[_currentPlatformIndex].GetComponent<Platform>().platformType;

            switch (platformType)
            {
                case PlatformType.Normal:
                    _rigidbody.linearVelocity = direction * jumpSpeed;
                    break;
                case PlatformType.Slippery:
                    StartCoroutine(DelayJump(direction, jumpSpeed, 60f / bpm)); // Delay jump by one beat
                    break;
                case PlatformType.Bouncy:
                    _rigidbody.linearVelocity = direction * jumpSpeed * 0.5f; // Decrease speed for bouncy platforms
                    break;
                // Add more cases as needed
            }

        }
    }

    IEnumerator DelayJump(Vector3 direction, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        _rigidbody.linearVelocity = direction * speed;
    }

   public void OnCollisionStay2D(Collision2D collider)
    {
        Platform platform = collider.gameObject.GetComponent<Platform>();
        if (platform != null)
        {
            StartCoroutine(WaitForClick());
            JumpToNextPlatform();
        }
    }


    public IEnumerator WaitForClick()
    {
        yield return new WaitForSeconds(2f);

        _platformManager.StopGame();
    }

} 

