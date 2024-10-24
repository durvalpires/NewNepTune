using UnityEngine;

[CreateAssetMenu(fileName = "WorldChallengeSO", menuName = "Scriptable Objects/v2_WorldChallengeSO")]
public class WorldChallengeSO : ScriptableObject
{
    public string id;
    public Sprite image;
    public string title;
    public LevelSO[] levels;
}
