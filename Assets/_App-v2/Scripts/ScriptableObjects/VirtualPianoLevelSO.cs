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
    public LevelHandType handType = LevelHandType.LeftHand;
}

public enum LevelHandType
{
    LeftHand = 1,
    RightHand = 2,
    Both = 3
}