using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PianoManag : MonoBehaviour
{
    private AudioSource _audioSource;
   
   private Collider2D _collider2D;
   private Renderer platformSprite;
   public bool gameStarted;
   
   
   public GameObject finalPanel;
    void Start()
    {
        _collider2D = gameObject.GetComponent<Collider2D>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    
  
   
  

 /*  private void OnCollisionEnter2D(Collision2D other)
   {
       
       if (other.gameObject.CompareTag("Player"))
       {
           re
       }
   }*/
 private void OnTriggerEnter2D(Collider2D other)
 {
     _audioSource.Play();
     gameStarted = true;
 }
}
