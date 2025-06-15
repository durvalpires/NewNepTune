using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "VirtualPianoLevelSO", menuName = "Scriptable Objects/v2_VirtualPianoLevelSO")]
public class VirtualPianoLevelSO : LevelSO
{
    public AssetReference songXml;
    public AssetReference songClip;
    public AssetReference backgroundClip;
    public bool isTutorial;
    public int introBeats = 2;
    public LevelHandType handType = LevelHandType.Left;
}

public enum LevelHandType
{
    Left = 1,
    Right = 2,
    Both = 3
}