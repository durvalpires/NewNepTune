using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoSingleton<SceneManagerScript>
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MainMenu()
    {
        Debug.Log("ana ekrana gitme butonuna basıldı."); // Butona başarıyla basıldığını doğrula
        SceneManager.LoadScene("MainLevelSelect");
        
    }

    public void Profile()
    {
        SceneManager.LoadScene("Profile");
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
   
    public void Level1Game1()
    {
        SceneManager.LoadScene("Level1Game1");
    }
}
