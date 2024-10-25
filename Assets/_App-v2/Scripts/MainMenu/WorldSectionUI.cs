using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldSectionUI : MonoBehaviour
{
    public Action<AllWorldsSO.WorldSections> onClick;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Image image;
    private AllWorldsSO.WorldSections _data;
    public void Init(AllWorldsSO.WorldSections data)
    {
        _data = data;
        image.sprite = data.image;
        title.text = data.title;
    }

    public void OnClick()
    {
        onClick.Invoke(_data);
    }
}
