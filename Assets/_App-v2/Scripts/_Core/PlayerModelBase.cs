using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

    public class PlayerModelBase
    {
        protected static PlayerData _player;
        protected static string _currentProfileKey = "";
        protected static PlayerData Data
        {
            get
            {
                if (_player == null)
                {
                    _currentProfileKey = ProfilesController.CurrentProfileKey;
                    Debug.LogWarning("CurrentProfileKey: " + _currentProfileKey);
#if UNITY_WEBGL && !UNITY_EDITOR
                    var userData = LocalStorageManager.LoadData(_currentProfileKey);
                    if(userData == null || !userData.Contains(":")) 
                        {
                            Debug.LogWarning("Creating new player data although it has found currentProfileKey");
                            return _player = new PlayerData();
                        }
                        
                    _player = JsonUtility.FromJson<PlayerData>(userData);
#else
                    if(PlayerPrefs.HasKey(_currentProfileKey))
                    {
                        var userData= PlayerPrefs.GetString(_currentProfileKey, "");
                        if(!userData.Contains(":")) 
                        {
                            Debug.LogWarning("Creating new player data although it has found currentProfileKey");
                            return _player = new PlayerData();
                        }
                        _player = JsonUtility.FromJson<PlayerData>(userData);
                    }
                    else
                    {
                        Debug.LogWarning("Creating new player data");
                        _player = new PlayerData();
                    }
#endif
                    
                }
                return _player;
            }
        }

        public static void SwitchToCurrentProfile()
        {
            SaveData();
            _player = null;
        }
        public static void SaveData()
        {
            var data = Json.Serialize(Data);
            if(string.IsNullOrEmpty(data)) return;
            Debug.Log("[SAVE] Data Saved:" + data.Substring(0,Mathf.Min(data.Length,100)));
#if UNITY_WEBGL && !UNITY_EDITOR
            LocalStorageManager.SaveData(_currentProfileKey, data);
#else
            PlayerPrefs.SetString(_currentProfileKey, data);
            PlayerPrefs.Save();
#endif
            
        }
        

        public static string GetCustomData(string key, string defaultValue = "")
        {
            Dictionary<string, object> dataDict = AllCustomData;
            if (dataDict.ContainsKey(key)) 
                return dataDict[key].ToString();
            return defaultValue;
        }
        
        private static Dictionary<string, object> AllCustomData
        {
            get
            {
                if (string.IsNullOrEmpty(Data.customUserData)) return new Dictionary<string, object>();
                Dictionary<string, object> dataDictFromJson = Json.Deserialize(Data.customUserData) as  Dictionary<string, object>;
                return dataDictFromJson;
            }
        }
        public static void SetCustomData(string key, string value)
        {
            Dictionary<string, object> dataDict = AllCustomData;

            if (dataDict.ContainsKey(key)) 
                dataDict[key] = value;
            else
                dataDict.Add(key, value);
            Data.customUserData = Json.Serialize(dataDict);
            SaveData();
        }
        protected class PlayerData
        {
            public string profileId = ""; // could use for switching profiles in future - creating of ProfileController is needed
            public string playerName = "";
            public bool IsSoundOn = true;
            public bool IsMusicOn = true;
            public string customUserData = "";
        }
    }
