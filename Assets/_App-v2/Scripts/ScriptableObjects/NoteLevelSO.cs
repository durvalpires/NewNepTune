using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "NoteLevelSO", menuName = "Scriptable Objects/v2_NoteLevelSO")]
public class NoteLevelSO : LevelSO
{
    public string note;
    public PopupStyle popupStyle;
    public enum PopupStyle
    {
        NoteOnCard,
        NotesOnPiano
    }

}
