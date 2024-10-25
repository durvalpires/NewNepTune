using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelsInitController : MonoBehaviour
{
    [SerializeField] private Image background;
    private void Start()
    {
        var data = TempDataStorage.GetData<AllWorldsSO.WorldSections>("Section");
        Sprite sprite = Resources.Load<Sprite>(data.sectionBGImageName);

        if (sprite != null)
            background.sprite = sprite;
        else
            Debug.LogError("Sprite not found in Resources folder!");
    }
}
