using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void MainMenu()
    {
        Debug.Log("ana ekrana gitme butonuna basıldı."); // Butona başarıyla basıldığını doğrula
        SceneManager.LoadScene("MainLevelSelect");
        
    }

    public void Achievements()
    {
        SceneManager.LoadScene("Achievements");
    }

    public void Profile()
    {
        SceneManager.LoadScene("Profile");
    }

    public void MiniGame1()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void RegisterLoginScene()
    {
        //burda oyuncu student veya teacher, hangisine tıkladığının bilgisini almamız gerekiyor.
        SceneManager.LoadScene("RegisterLoginScene");
    }

    public void CharacterSelectionScene()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void MainSelection3DScene()
    {
        SceneManager.LoadScene("MainSelectionScene");
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void Level1Game1()
    {
        SceneManager.LoadScene("Level1Game1");
    }
    public void KimMilyoner()
    {
        SceneManager.LoadScene("KimMilyoner");
    }
    public void MemoryCard()
    {
        SceneManager.LoadScene("MemoryCard");
    }

    public void SoundQuiz()
    {
        SceneManager.LoadScene("SoundQuizScene");
    }
}
