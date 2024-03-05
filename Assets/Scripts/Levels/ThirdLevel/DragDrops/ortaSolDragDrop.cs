using UnityEngine;

public class ortaSolDragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Vector3 snapPosition; // Hedef konum, Inspector'dan ayarlanabilir
    public float snapRadius = 0.5f; // Yapışma mesafesi

    void Update()
    {
        if (isDragging)
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            currentPosition.z = 0;
            transform.position = currentPosition;
        }
    }

    void OnMouseDown()
    {
        if (!isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - mousePosition;
            offset.z = 0;
            isDragging = true;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToPosition();
    }

    void SnapToPosition()
    {
        float distance = Vector3.Distance(transform.position, snapPosition);
        if (distance <= snapRadius)
        {
            transform.position = snapPosition;
        }
    }
}
