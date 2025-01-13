using System.Linq;
using Levels.SelectionMinigame;
using UnityEngine;

namespace _App_v2.Scripts.Levels
{
    public class ImageSelectionLevelManager : SelectionMinigame
    {
        private void Awake()
        {
            var data = TempDataStorage.GetSceneData<ImageSelectionLevelSO>();
            if(data == null)
                Debug.LogError("No data found for current level");

            contentType = data.type.ToString();
            correctAnswerSpriteName = data.note;
            levelToReturn = PlayerModel.GeneralConfig.levelsScene;
            var spritesList = Resources.LoadAll<Sprite>($"SelectionMinigame/{contentType}/").ToList();
            spritesList.Shuffle();
            _sprites = spritesList.ToArray();
        }
    }
}