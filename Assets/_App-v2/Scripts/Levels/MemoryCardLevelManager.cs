using System;
using UnityEngine;

public class MemoryCardLevelManager : SceneController
{
    private void Awake()
    {
        var data = TempDataStorage.GetSceneData<MemoryCardLevelSO>();
        var images = new Sprite[data.imagesNames.Length];
        for (int i = 0; i < data.imagesNames.Length; i++)
        {
            images[i] = Resources.Load<Sprite>(data.imagesNames[i]);
        }
        this.images = images;
    }
}
