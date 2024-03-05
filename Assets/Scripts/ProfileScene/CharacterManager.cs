using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   public CharacterDatabase characterDB;
   public SpriteRenderer characterSprite;
   public SpriteRenderer hatSprite;

   private int selectedOption = 0;
   private int hatSelected = 0;

   private void Start()
   {
      if (PlayerPrefs.HasKey("SelectedCharacter"))
      {
         selectedOption = 0;
      }
      else if (PlayerPrefs.HasKey("SelectedHat"))
      {
         hatSelected = 0;
      }
      else
      {
         Load();
      }
      UpdateCharacter(selectedOption);
      UpdateHat(hatSelected);
   }

   public void CharacterNextOption()
   {
      selectedOption++;

      if (selectedOption >= characterDB.CharacterCount)
      {
         selectedOption = 0;
      }
      UpdateCharacter(selectedOption);
      Save();
   }

   public void CharacterBackOption()
   {
      selectedOption--;

      if (selectedOption < 0)
      {
         selectedOption = characterDB.CharacterCount - 1;
      }
      UpdateCharacter(selectedOption);
      Save();
   }

   private void UpdateCharacter(int selectedOption)
   {
      Character character = characterDB.GetCharacter(selectedOption);
      characterSprite.sprite = character.characterSprite;
   }

   private void UpdateHat(int selectedHat)
   {
      Character hat = characterDB.GetHat(selectedHat);
      hatSprite.sprite = hat.characterSprite;

   }

   public void HatBackOption()
   {
      hatSelected--;

      if (hatSelected < 0)
      {
         hatSelected = characterDB.HatCount - 1;
      }
      UpdateHat(hatSelected);
      Save();
   }
   
   public void HatNextOption()
   {
      hatSelected++;

      if (hatSelected >= characterDB.HatCount)
      {
         hatSelected = 0;
      }
      UpdateHat(hatSelected);
      Save();
   }

   private void Load()
   {
      selectedOption = PlayerPrefs.GetInt("SelectedCharacter");
      hatSelected = PlayerPrefs.GetInt("SelectedHat");
   }
   
   private void Save()
   {
      PlayerPrefs.SetInt("SelectedCharacter", selectedOption);
      PlayerPrefs.SetInt("SelectedHat", hatSelected);
   }
}
