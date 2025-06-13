using System;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;

[Serializable]
public class AccountDataDB : IDBData
{
    public string email = "";
    public AccountType accountType;
    public string profileId = "";
    public string playerName = "";
    public bool IsSoundOn = true;
    public bool IsMusicOn = true;
    //public string customUserData = "";
    public float totalPlayTime = 0f;
    
    
    public AccountDataDB()
    {
        accountType = AccountType.Unknown;
        profileId = "TestProfile";
        playerName = "TestPlayer";
        IsSoundOn = true;
        IsMusicOn = true;
        //customUserData = "";
        totalPlayTime = 0f;
    }
}

public enum AccountType
{
    Student = 1,
    Teacher = 2,
    Admin = 3,
    Guest = 4,
    Unknown = -1 
}