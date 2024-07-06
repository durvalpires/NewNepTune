using Extensions;
using Levels.SelectionMinigame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoSingleton<SceneManagerScript>
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log($"Changed scene to {sceneName}");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainLevelSelect");
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
    
    // sceneName format: sceneToLoad,sceneNote,sceneToReturn
    public void LoadSelectionMinigame(string sceneName)
    {
        string[] sceneParts = sceneName.Split(',');

        if (sceneParts.Length != 3)
        {
            Debug.LogError("Invalid sceneName format. Expected format: sceneToLoad,sceneNote,sceneToReturn");
            return;
        }

        string sceneToLoad = sceneParts[0];
        string sceneNote = sceneParts[1];
        string sceneToReturn = sceneParts[2];
        
        SceneManager.LoadSceneAsync(sceneToLoad);

        Scene scene = SceneManager.GetSceneByName(sceneToLoad);
        Debug.Log(scene.isLoaded);

        while (!scene.isLoaded)
        {
            
        }
        
        //Your code must also be tolerant of other objects not being immediately ready,
        //as all scene loading completes at the end of the current frame.

        GameObject managersGameObject = GameObject.FindWithTag("ManagersGameObject");
        SelectionMinigame selectionMinigame = managersGameObject.GetComponent<SelectionMinigame>();
        selectionMinigame.levelToReturn = sceneToReturn;
        selectionMinigame.correctAnswerSpriteName = sceneNote;

        //set up level
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
