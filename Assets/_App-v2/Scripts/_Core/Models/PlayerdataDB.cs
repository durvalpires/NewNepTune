using System;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;

[Serializable]
public class PlayerDataDB : IDBData
{
    public string profileId = "";
    public string playerName = "";
    public bool IsSoundOn = true;
    public bool IsMusicOn = true;
    public string customUserData = "";
    public float totalPlayTime = 0f;
}