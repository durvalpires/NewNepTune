using UnityEngine;

public class CameraMovementG1L1 : MonoBehaviour
{
    public float speed = 2f; // Kameranın hareket hızı
    private float startX = -13f; // Başlangıç X pozisyonu
    private float endX = 30f; // Bitiş X pozisyonu

    void Start()
    {
        // Kamera pozisyonunu başlangıç noktasına ayarla
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Kamera mevcut pozisyonundan, belirlenen hızla sağa doğru hareket et
        transform.position += Vector3.right * speed * Time.deltaTime;

        // Kamera, belirlenen bitiş X pozisyonuna ulaştığında hareketi durdur
        if (transform.position.x >= endX)
        {
            // Hızı sıfırla veya isteğe bağlı olarak başka bir işlem yap
            speed = 0;
        }
    }
}
