using UnityEngine;

[CreateAssetMenu(fileName = "ImageSelectionLevelSO", menuName = "Scriptable Objects/v2_ImageSelectionLevelSO")]
public class ImageSelectionLevelSO : LevelSO
{
    public string note;
    public ImageSelectionType type = ImageSelectionType.Notes;
}

public enum ImageSelectionType
{
    Notes = 0,
    Rhythms = 1
}
