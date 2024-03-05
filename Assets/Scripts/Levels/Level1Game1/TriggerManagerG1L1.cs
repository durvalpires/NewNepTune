using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class TriggerManagerG1L1 : MonoBehaviour
{
    public bool notaBasilmalimi;
    public TextMeshProUGUI debugText;
    void Start()
    {
        notaBasilmalimi = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Girdi");
        notaBasilmalimi = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Çıktı");
        notaBasilmalimi = false;
    }
    public void DoNotaKontrol()
    {
        if(notaBasilmalimi)
        {
            StartCoroutine(DebugTextString("dogru zamanda bastin")); 
        }
        else
        {
            StartCoroutine(DebugTextString("yanlis zamanda bastin"));
        }
    }
    IEnumerator DebugTextString(string debugTextNumerator)
    {
        debugText.text = debugTextNumerator;
        yield return new WaitForSeconds(0.7f);
        debugText.text = " ";
    }
}
