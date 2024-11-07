using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualPianoKeyController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private string note;

    private TextMeshProUGUI noteTxt;
    private Image backgroundImg;

    private Color myColor;

    private void Awake()
    {
        noteTxt = GetComponentInChildren<TextMeshProUGUI>();
        backgroundImg = GetComponent<Image>();
    }

    public string GetNote() { return note; }

    public void SetKeyColor(Color newColor)
    {
        myColor = newColor;
        noteTxt.color = newColor;
    }

    public void SetKeyColorActive()
    {
        backgroundImg.DOColor(myColor, 0.2f);
    }

    public void SetKeyColorNormal()
    {
        backgroundImg.DOColor(Color.white, 0.5f);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //SetKeyColorActive();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        SetKeyColorNormal();
    }
}
