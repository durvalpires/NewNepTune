using UnityEngine;
using System;
using System.Collections;

public class MetronomManager : MonoBehaviour
{
    public float[] tempos; // Farklı tempoları saklamak için bir dizi
    public float beatInterval; // Her vuruş arasındaki süreyi saklamak için bir değişken
    public Transform objectTransform; // Hareket ettirilecek nesnenin transform bileşenini saklamak için bir değişken
    public Vector3 position1; // Nesnenin ilk pozisyonu
    public Vector3 position2; // Nesnenin ikinci pozisyonu

    private int currentTempoIndex; // Seçili metronom temposunun dizideki indisini saklamak için bir değişken
    private float timer; // Vuruş zamanlamasını kontrol etmek için bir zamanlayıcı

    void Start()
    {
        currentTempoIndex = 0; // Başlangıçta ilk tempo seçili
        tempos = new float[] { 60, 90, 120, 150 }; // Temposu diziye ekleme
        beatInterval = 60 / tempos[currentTempoIndex]; // İlk metronom temposuna göre vuruş aralığını hesaplama
        timer = 0; // Zamanlayıcıyı sıfırlama
    }

    void Update()
    {
        timer += Time.deltaTime; // Zamanlayıcıyı her karede güncelleme

        if (timer >= beatInterval) // Vuruş zamanı geldiğinde
        {
            timer = 0; // Zamanlayıcıyı sıfırlama
            objectTransform.position = (objectTransform.position == position1) ? position2 : position1; // Nesnenin pozisyonunu değiştirme
        }
    }

    public void SetTempo(int index) // Butonlara tıklandığında tetiklenecek fonksiyon
    {
        currentTempoIndex = index; // Seçilen metronom temposunun indisini güncelleme
        beatInterval = 60 / tempos[currentTempoIndex]; // Yeni metronom temposuna göre vuruş aralığını hesaplama
    }
}
