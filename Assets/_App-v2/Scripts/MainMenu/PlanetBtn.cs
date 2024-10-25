using System;
using UnityEngine;
using UnityEngine.UI;

public class PlanetBtn : MonoBehaviour
{
    public Action<AllWorldsSO.WordData> onClickAction;
    [SerializeField] private Image image;
    
    private AllWorldsSO.WordData _data;
    
    public void Init(AllWorldsSO.WordData data)
    {
        image.sprite = data.worldSprite;
        _data = data;
    }
    public void OnClick()
    {
        onClickAction?.Invoke(_data);
    }
}
