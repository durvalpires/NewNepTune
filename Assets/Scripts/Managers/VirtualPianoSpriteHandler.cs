using System.Collections;
using System.Collections.Generic;
using Instruments;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPianoSpriteHandler : MonoBehaviour
{
    public Image spriteRenderer;
    
    private PianoButtons _pianoButtons;
    public Sprite trueSprite;
    public Sprite baseSprite;
    // Start is called before the first frame update
    void Start()
    {
        _pianoButtons = FindObjectOfType<PianoButtons>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_pianoButtons.pressedTrue)
        {
            spriteRenderer.sprite = trueSprite;
        }
        else
        {
            spriteRenderer.sprite = baseSprite;
        }
        
    }
}
