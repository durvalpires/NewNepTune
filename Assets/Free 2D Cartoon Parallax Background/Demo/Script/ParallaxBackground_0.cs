using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    [Header("Layer Setting")]
    public float[] Layer_Speed = new float[7];
    public GameObject[] Layer_Objects = new GameObject[7];

    private Transform _camera;
    private Vector2 lastMousePosition;
    private Vector2 currentMousePosition;
    private bool isDragging = false;
    private float[] startPos = new float[7];
    private float boundSizeX;
    private float sizeX;

    void Start()
    {
        _camera = Camera.main.transform;
        sizeX = Layer_Objects[0].transform.localScale.x;
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            startPos[i] = Layer_Objects[i].transform.position.x;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
        UpdateParallaxEffect();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentMousePosition = Input.mousePosition;
            float distance = lastMousePosition.x - currentMousePosition.x; // Tersi yönde hareket için işareti değiştir

            MoveCamera(distance);

            lastMousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastMousePosition = touch.position;
                    isDragging = true;
                    break;
                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        currentMousePosition = touch.position;
                        float distance = lastMousePosition.x - currentMousePosition.x; // Tersi yönde hareket için işareti değiştir

                        MoveCamera(distance);

                        lastMousePosition = currentMousePosition;
                    }
                    break;
                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }

    void MoveCamera(float distance)
    {
        float speed = 0.005f; // Hızı ayarlayın. Bu değer deneme yanılma ile en uygun hale getirilebilir.
        _camera.position += new Vector3(distance * speed, 0, 0);
    }

    void UpdateParallaxEffect()
    {
        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            float temp = (_camera.position.x * (1 - Layer_Speed[i]));
            float distance = (_camera.position.x - startPos[i]) * Layer_Speed[i];
            Layer_Objects[i].transform.position = new Vector2(startPos[i] - distance, Layer_Objects[i].transform.position.y); // Tersi yönde hareket için işareti değiştir
        }
    }
}
