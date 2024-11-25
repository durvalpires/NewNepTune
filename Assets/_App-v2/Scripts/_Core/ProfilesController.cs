using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// guest profile has nickName "GUEST" used by default on start
/// Public methods:
/// ProfilesController.CurrentProfile - get current profile
/// ProfilesController.GetAllProfiles  - return List<ProfileProps> of all profiles
/// ProfilesController.CreateProfile(nickName, password) - create new profile
/// ProfilesController.SwitchProfile(nickName, password) - switch to profile (login)
/// ProfilesController.SwitchToGuest() - switch to guest
/// ProfilesController.CurrentProfileKey - used in PlayerModel as saving key
/// </summary>
public class ProfilesController
{
    private static ProfilesData _profiles;
    private const string PlPrefsDataKey = "_pl_prefs_profiles_data_v0";
    private const string guestNickName = "GUEST";

    private static ProfilesData Data
    {
        get
        {
            if (_profiles == null)
            {
                if(PlayerPrefs.HasKey(PlPrefsDataKey))
                {
                    var userData= PlayerPrefs.GetString(PlPrefsDataKey, "");
                    if(!userData.Contains(":")) 
                        return _profiles = new ProfilesData();
                    _profiles = JsonUtility.FromJson<ProfilesData>(userData);
                }
                else
                {
                    _profiles = new ProfilesData();
                }
            }
            return _profiles;
        }
    }
    
    public static void CreateProfile(string nickName, string password)
    {
        //check if nickname already exists
        if (AllSavedProfiles.ContainsKey(nickName))
        {
            Debug.Log( "Nick name already exists");
            return;
        }

        //check if password is not empty
        if (string.IsNullOrEmpty(password))
        {
            Debug.Log("Empty Password provided");
        }

        ProfileProps pr = new ProfileProps();
        pr.NickName = nickName;
        pr.PasswordHash = HashPassword(password);

        AddProfile(nickName, Json.Serialize(pr));
        SwitchProfile(nickName, password);
    }
    public class ProfileProps
    {
        public string NickName;
        public string PasswordHash;
    }

    public static string CurrentProfileKey => CurrentProfile.NickName+CurrentProfile.PasswordHash;

    public static List<ProfileProps> GetAllProfiles()
    {
        var list = new List<ProfileProps>();
        var all = AllSavedProfiles;
        foreach (var profile in all)
        {
            var dat = JsonUtility.FromJson<ProfileProps>(profile.Value as string);
            list.Add(dat);
        }
        return list;
    }
    private static Dictionary<string, object> AllSavedProfiles
    {
        get{
            
            if (!string.IsNullOrEmpty(Data.AllProfilesData))
            {
                return Json.Deserialize(Data.AllProfilesData) as  Dictionary<string, object>;
            }
            return new Dictionary<string, object>();
        }
    }

    public static ProfileProps CurrentProfile
    {
        get
        {
            if (string.IsNullOrEmpty(Data.CurrentProfile))
            {
                if (GetProfileData(guestNickName) == null)
                {
                    CreateProfile(guestNickName, guestNickName);
                    SwitchProfile(guestNickName, guestNickName);
                }
                
            }
            return GetProfileData(Data.CurrentProfile);
        }
        
    } 
    private static ProfileProps GetProfileData(string key)
    {
        if(string.IsNullOrEmpty(key) || !AllSavedProfiles.ContainsKey(key)) 
            return null;

        return JsonUtility.FromJson<ProfileProps>(AllSavedProfiles[key] as string);
    }

    public static void SwitchToGuest()
    {
        if (!AllSavedProfiles.ContainsKey(guestNickName))
        {
            CreateProfile(guestNickName, guestNickName);
        }
        SwitchProfile(guestNickName, guestNickName);
    }
    public static void SwitchProfile(string nickName, string pass)
    {
        
        if(!AllSavedProfiles.ContainsKey(nickName)) 
        {
            //profile not exists
            Debug.Log("Profile not exists: " + nickName);
            return;
        }

        if (GetProfileData(nickName).PasswordHash != HashPassword(pass))
        {
            // wrong password
            Debug.Log("Wrong password for: " + nickName);
            return;
        }
        
        Data.CurrentProfile = nickName;
        PlayerModelBase.SwitchToCurrentProfile();
        SaveData();
        Debug.Log("Switched to: "+nickName);
    }
    private static void AddProfile(string profileIdKey, string value)
    {
        Dictionary<string, object> dataDict = AllSavedProfiles;

        if (dataDict.ContainsKey(profileIdKey)) 
            dataDict[profileIdKey] = value;
        else
            dataDict.Add(profileIdKey, value);
        
        Data.AllProfilesData = Json.Serialize(dataDict);
        SaveData();
    }
    
    public static void SaveData()
    {
        var data = Json.Serialize(Data);
        if(string.IsNullOrEmpty(data)) return;
        Debug.Log("[SAVE] Data Saved:" + 
                  data.Substring(0,Mathf.Min(data.Length,100)));
        PlayerPrefs.SetString(PlPrefsDataKey, data);
    }
    private class ProfilesData
    {
        public string CurrentProfile = "";
        public string AllProfilesData = "";
    }
    private static string HashPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            StringBuilder hashStringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashStringBuilder.Append(b.ToString("x2"));
            }

            return hashStringBuilder.ToString();
        }
    }
}

