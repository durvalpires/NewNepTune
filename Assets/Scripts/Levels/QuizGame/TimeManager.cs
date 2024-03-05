using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //soruyu çözmek için gereken zaman
    [SerializeField] float timeToCompleteQuestion = 10f;
    //doğru cevabı göstermek için gereken zaman - diğer soruya geçiş
    [SerializeField] float timeToShowCorrectAnswer = 5f;
    //zamanı timervalue ile ölçelim. sprite ile eşleme burada yapılacak

    //yeni soruya geçme bool u
    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFraction; //sprite daki timer değerimiz
    float timerValue;
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if(isAnsweringQuestion) //soru cevaplandıysa
        {
            if(timerValue > 0)
            {
                fillFraction = timerValue / timeToCompleteQuestion; 
                /*bunu yapmamızın sebebi, sprite değeri 0 dan 1 e kadar bir değer. 
                o yüzden timervalue değerini toplam süreye bölersek, 0 ve 1 arasında bir 
                kesirli değer buluruz ve bunu fillfractiona eşitleriz*/
            }
            else //soru cevaplandıysa ve geri sayım 0 ise
            {
                isAnsweringQuestion = false; //soru cevap boolean ı deaktive et 
                timerValue = timeToShowCorrectAnswer;   //zamanı doğru cevap gösterme süresinden geriye say
            }
        }
        else //eğer soru cevaplanmadıysa
        {
            if(timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else //soru cevaplanmadıysa ve süre 0'ı geçtiyse
            {
                isAnsweringQuestion = true; //soru cevap boolean aktifleştir
                timerValue = timeToCompleteQuestion; //zamanı soru cevaplama süresinden geriye say
                loadNextQuestion = true; //yeni soruyu yükle.
            }
        }
        Debug.Log(isAnsweringQuestion + ": " + timerValue + " = " + fillFraction + "--" + loadNextQuestion);
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }
}
