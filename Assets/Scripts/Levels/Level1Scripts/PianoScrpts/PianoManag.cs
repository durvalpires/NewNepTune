using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class PianoManag : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}