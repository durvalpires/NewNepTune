using UnityEngine;

public class MusicBall : MonoBehaviour
{
    public GameObject ballPrefab;
    private GameObject _ball;
    private float _jumpTime;

    public void StartBall(float bpm, Vector3 startPosition)
    {
        _ball = Instantiate(ballPrefab, startPosition, Quaternion.identity);
        _jumpTime = 60f / bpm;
    }

    void Update()
    {
        if (_ball.transform.position.y <= 0)
        {
            _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, _jumpTime), ForceMode2D.Impulse);
        }
    }
}
    
