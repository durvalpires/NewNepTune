using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ChoiceBasedLevelData", menuName = "Scriptable Objects/ChoiceBasedLevelData")]
public class ChoiceBasedLevelData<T> : LevelData where T : UnityEngine.Object
{
    public T correctAnswer;
}
