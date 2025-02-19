using Cysharp.Threading.Tasks;
using Levels.SelectionMinigame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevlesSceneManagerScript : SceneManagerScript
{
    private GeneralConfigSO _generalConfigSo => PlayerModel.GeneralConfig;
    
    public override void LoadSelectionMinigame(string sceneName)
    {
        LoadSelectionMinigameAsync2(_generalConfigSo.selectionMiniGameScene,sceneName);
    }

    public override void MainMenu()
    {
        SceneManager.LoadScene(_generalConfigSo.planetsScene);
    }

    private static async UniTask LoadSelectionMinigameAsync2(string miniGameScene,string sceneName)
    {
        string[] sceneParts = sceneName.Split(',');

        if (sceneParts.Length != 2)
        {
            Debug.LogError("Invalid sceneName format. Expected format: sceneNote,sceneToReturn");
            return;
        }

        string sceneNote = sceneParts[0];
        string sceneToReturn = sceneParts[1];

        await LoadLevelAsync(miniGameScene);

        GameObject managersGameObject = GameObject.FindWithTag("MinigameManager");
        SelectionMinigame selectionMinigame = managersGameObject.GetComponent<SelectionMinigame>();
        selectionMinigame.levelToReturn = sceneToReturn;
        selectionMinigame.correctAnswerGOName = sceneNote;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
