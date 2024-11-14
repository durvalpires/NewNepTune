using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "VirtualPianoLevelSO", menuName = "Scriptable Objects/v2_VirtualPianoLevelSO")]
public class VirtualPianoLevelSO : LevelSO
{
    public AssetReference songXml;
    public AssetReference songClip;
    public AssetReference backgroundClip;
}
