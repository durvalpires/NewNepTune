using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlanetBtn : MonoBehaviour
{
    public Action<WorldSO, int> onClickAction;
    [SerializeField] private Image image;
    [SerializeField] private BoolUnityEvent onLocked;
    [SerializeField] private BoolUnityEvent onComplete;
    [SerializeField] private UITextPresenter titleTextPresenter;
    private bool isLocked = false;
    private WorldSO _data;
    private int _index = -1;
    
    public void Init(WorldSO data, int index)
    {
        _index = index;
        image.sprite = data.worldSprite;
        _data = data;
        titleTextPresenter.UpdateView(data.title);
        if (PlayerModel.IsWorldCompleted(data.id))
        {
            onComplete.Invoke(true);
        }
    }
    
    public void Lock()
    {
        isLocked = true;
        onLocked.Invoke(true);
    }
    public void OnClick()
    {
        if(isLocked) return;
        
        onClickAction?.Invoke(_data,_index);
    }
}
