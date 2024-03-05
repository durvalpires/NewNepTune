using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //textmeshpro componentini çekiyoruz.

public class QuizManager : MonoBehaviour
{
    [Header("Questions")]
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
        QuestionSO currentQuestionSO;
    [Header("Answers")]
        [SerializeField] GameObject[] answerButtons;
        int correctAnswerIndex;
        bool hasAnswerEarly = true; //soru daha önceden cevaplandı mı?
    [Header("Buttons")]
        [SerializeField] Sprite defaultAnswerSprite; //varsayılan butonun arkaplan resmi
        [SerializeField] Sprite correctAnswerSprite; //doğru cevabın arkaplan resmi
    [Header("Timer")]
        [SerializeField] Image timerImage;
        TimeManager timer;
    [Header("Scoring")]
        [SerializeField] TextMeshProUGUI scoreText;
        ScoreManager scoreKeeper;
    [Header("ProgressBar")]
        [SerializeField] Slider progressBar; //slider ımızı içeri alıyoruz.
    public bool isQuizComplete;

    void Awake()
    {
        timer = FindObjectOfType<TimeManager>();
        scoreKeeper = FindObjectOfType<ScoreManager>();
        progressBar.maxValue = questions.Count; //soruların toplam sayısını slider'ın max değerine eşitledik
        progressBar.value = 0; //baştaki değer 0.
    }

    void Update()
    {
        //zamanı sprite ile eşle
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if(progressBar.value == progressBar.maxValue)
            {
                //slider değeri fullenirse, yani çözülen sorular biterse aşağıdaki değeri true yap
                isQuizComplete = true;
                return;
            }
            hasAnswerEarly = false;
            GoNextQuestion(); //diğer soruya git
            timer.loadNextQuestion = false; //ve yeni soruya geçme bool unu tekrar sıfırla.
        }
        else if(!hasAnswerEarly && !timer.isAnsweringQuestion) //cevap önceden verilmediyse
        {
            DisplayAnswer(-1); //-1 diyerek otomatik olarak else değerini döndüreceğiz.
            SetButtonState(false);
        }
    }

    //soruyu gösterme kodlarını metot içerisine aldık
    void DisplayQuestion()
    {
        questionText.text = currentQuestionSO.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>(); //butonu al, buton içindeki child objesini yani doğal olarak texti al ve değiştir.
            buttonText.text = currentQuestionSO.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnswerEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer(); //bu kısmı anlamadım. cevap seçilince neden zaman duruyor anlamadım çünkü gameplay de zaman durmuyor
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%"; //cevabı seçince skorumuz işleyecek.
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;
        if(index == currentQuestionSO.GetCorrectAnswerIndex()) //eğer cevap doğruysa
        {
            questionText.text = "Correct!"; //soru yazısını değiştir
            buttonImage = answerButtons[index].GetComponent<Image>(); //butonların arkaplan resmini çek
            buttonImage.sprite = correctAnswerSprite; //eğer doğru cevap index == currentQuestionSO.getcorr.... şeklinde ise doğru cevap sprite'ını arkaplan yap.
            scoreKeeper.IncrementCorrectAnswers(); //doğru cevap skoru artırır
        }
        else //eğer cevap yanlışsa
        {
            /*questionText.text ="False Answer!";
            Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>(); //doğru olan cevabı correctanswer sprite ı ile çevreliyoruz
            buttonImage.sprite = correctAnswerSprite; */
            //yukarıda yaptığım da doğru ama aşağıda kursta yapılan kod var o yüzden üst tarafı yorum satırına aldım.
            correctAnswerIndex = currentQuestionSO.GetCorrectAnswerIndex();
            string correctanswer = currentQuestionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was;\n" + correctanswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        //cevap seçildikten sonra butonun kullanılabilirliğini kapatıyoruz.
    }

    void GoNextQuestion()
    {
        if(questions.Count > 0) //soru sayımız 0 dan fazla ise
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++; //her soru çözülüşte progressbar değeri bir artıyor.
            scoreKeeper.IncrementQuestionSeen(); //diğer soruya geçmek, görülen soru sayısını artırır
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count); //kaç soru varsa aralarından rastgele olarak döndürecek
        currentQuestionSO = questions[index];
        if(questions.Contains(currentQuestionSO)) //eğer şimdiki soru listede varsa...
            questions.Remove(currentQuestionSO); //soru görülünce o soruyu listeden sil ki bir daha çıkmasın
    }
    //yeni soruya geçince sprite lar sıfırlanmalı
    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    //butonun açılıp kapanma durumunu kontrol altına aldık
    void SetButtonState(bool butonDurum)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = butonDurum;
        }
    }
}
