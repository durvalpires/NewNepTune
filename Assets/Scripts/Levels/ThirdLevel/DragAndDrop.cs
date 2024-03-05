using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Vector3 snapPosition; // Hedef konum, Inspector'dan ayarlanabilir
    public float snapRadius = 0.5f; // Yapışma mesafesi
    public static int magneticTracker = 6;
    public GameObject congratsText;
    [Header("Sounds")]
    public AudioSource DraggingSound;
    public AudioClip DragDoneSound;

    void Update()
    {
        if (isDragging)
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            currentPosition.z = 0;
            transform.position = currentPosition;
            //Debug.Log(currentPosition);
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
            magneticTracker--;
            Debug.Log(magneticTracker);
            DraggingSound.PlayOneShot(DragDoneSound);
            if(magneticTracker <= 0)
            {
                Debug.Log("oyun bitti");
                congratsText.SetActive(true);
            }
        }
    }
}
