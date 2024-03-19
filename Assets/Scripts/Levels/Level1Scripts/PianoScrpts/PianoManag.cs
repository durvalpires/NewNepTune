using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PianoManag : MonoBehaviour
{
   public GameObject movingObject;
   private Collider2D _collider2D;
   private Renderer platformSprite;
   
   
   public GameObject finalPanel;
    void Start()
    {
        _collider2D = gameObject.GetComponent<Collider2D>();
    }

    
   /*public void HighLightPlatform()
   {
       if (followerScript.platforms.Count > 0 && followerScript.currentPlatformIndex < followerScript.platforms.Count)
       { 
           platformSprite = followerScript.platforms[followerScript.currentPlatformIndex].GetComponent<Renderer>();
           platformSprite.material.color = Color.red;
       }
   }*/
   
   private void OnTriggerEnter2D(Collider2D other)
   {
         if (other.gameObject.CompareTag("Player")) 
         {
              finalPanel.SetActive(true);
         }
   }
  
 
   
}
