using System;
using UnityEngine;

public class PlanetBtn : MonoBehaviour
{
    public Action<PlanetBtn> onClickAction;

    public void OnClick()
    {
        onClickAction?.Invoke(this);
    }
}
