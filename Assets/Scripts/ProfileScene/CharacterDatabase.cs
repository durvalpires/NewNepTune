using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
   public Character[] character;
   public Character[] hat;

   public int CharacterCount
   {
      get
      {
         return character.Length;
      }
   }

   public int HatCount
   {
      get
      {
         return hat.Length;
      }
   }

   public Character GetCharacter(int index)
   {
      return character[index];
   }

   public Character GetHat(int _hatindex)
   {
      return hat[_hatindex];
   }
}
