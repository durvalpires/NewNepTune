using UnityEngine;

public class CameraSwipeControl : MonoBehaviour
{
    private Vector2 lastMousePosition;
    private Vector2 currentMousePosition;
    private bool isDragging = false;

    void Update()
    {
        #if UNITY_EDITOR
        HandleMouseInput();
        #else
        HandleTouchInput();
        #endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && IsCameraInBounds())
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentMousePosition = Input.mousePosition;
            float distance = currentMousePosition.x - lastMousePosition.x;

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
                if (IsCameraInBounds())
                {
                    lastMousePosition = touch.position;
                    isDragging = true;
                }
                break;
            case TouchPhase.Moved:
                if (isDragging)
                {
                    currentMousePosition = touch.position;
                    float distance = currentMousePosition.x - lastMousePosition.x;

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

    bool IsCameraInBounds()
    {
        return transform.position.x >= -30f && transform.position.x <= 30f;
    }

    void MoveCamera(float distance)
    {
        // Kamera hareket hızını ayarlayabilirsiniz. 
        // 'speed' değeri, hareketin hızını kontrol eder.
        float speed = 0.05f;
        Vector3 newPosition = transform.position + new Vector3(-distance * speed, 0, 0);

        // Kameranın yeni pozisyonunun sınırlar içinde olup olmadığını kontrol et
        if (newPosition.x >= -30f && newPosition.x <= 30f)
        {
            transform.position = newPosition;
        }
    }
}
