using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Levels.Level1Game1;
using UnityEngine;

public class PianoManag : MonoBehaviour
{
    public AudioSource audioSource;
    private TriggerManagerG1L1 triggerManager;

    private void Start()
    {
        triggerManager = FindObjectOfType<TriggerManagerG1L1>();
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}