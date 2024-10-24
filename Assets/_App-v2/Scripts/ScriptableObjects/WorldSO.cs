using UnityEngine;

[CreateAssetMenu(fileName = "WorldSO", menuName = "Scriptable Objects/v2_WorldSO")]
public class WorldSO : ScriptableObject
{
     public string id;
     public string title;
     public Sprite planetSprite;
     public WorldChallengeSO[] levels;

}
