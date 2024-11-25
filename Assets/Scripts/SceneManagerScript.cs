using Cysharp.Threading.Tasks;
using Extensions;
using Levels.SelectionMinigame;
using Minigames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoSingleton<SceneManagerScript>
{
    // [SerializeField] private GeneralConfigSO generalConfig;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log($"Changed scene to {sceneName}");
    }

    public virtual void MainMenu()
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

    public virtual void LoadSelectionMinigame(string sceneName)
    {
        LoadSelectionMinigameAsync(sceneName);
    }

    public void LoadInstrumentGuessMinigame(string sceneName)
    {
        LoadMusicInstrumentGuessAsync(sceneName);
    }

    protected static async UniTask LoadLevelAsync(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    // sceneName format: sceneNote,sceneToReturn
    private static async UniTask LoadSelectionMinigameAsync(string sceneName)
    {
        string[] sceneParts = sceneName.Split(',');

        if (sceneParts.Length != 2)
        {
            Debug.LogError("Invalid sceneName format. Expected format: sceneNote,sceneToReturn");
            return;
        }

        string sceneNote = sceneParts[0];
        string sceneToReturn = sceneParts[1];

        await LoadLevelAsync("SelectionMinigame");

        GameObject managersGameObject = GameObject.FindWithTag("MinigameManager");
        SelectionMinigame selectionMinigame = managersGameObject.GetComponent<SelectionMinigame>();
        selectionMinigame.levelToReturn = sceneToReturn;
        selectionMinigame.correctAnswerSpriteName = sceneNote;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    private static async UniTask LoadMusicInstrumentGuessAsync(string levelToReturn)
    {
        string sceneToReturn = levelToReturn;

        await LoadLevelAsync("InstrumentGuess");

        GameObject managersGameObject = GameObject.FindWithTag("MinigameManager");
        InstrumentGuess instrumentMinigame = managersGameObject.GetComponent<InstrumentGuess>();
        instrumentMinigame.levelToReturn = sceneToReturn;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public void LoadLevelListScene()
    {
        SceneManager.LoadScene(PlayerModel.GeneralConfig.levelsScene);
    }
}