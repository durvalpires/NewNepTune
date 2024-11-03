using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Antari
{
    public class PlayerModelBase
    {
        private static PlayerData _player;
        private const string PlPrefsDataKey = "_pl_prefs_data_v0";
        protected static PlayerData Data
        {
            get
            {
                if (_player == null)
                {
                    if(PlayerPrefs.HasKey(PlPrefsDataKey))
                    {
                        var userData= PlayerPrefs.GetString(PlPrefsDataKey, "");
                        if(!userData.Contains(":")) 
                            return _player = new PlayerData();
                        _player = JsonUtility.FromJson<PlayerData>(userData);
                    }
                    else
                    {
                        _player = new PlayerData();
                    }
                }
                return _player;
            }
            
        }

        public static void SaveData()
        {
            var data = Json.Serialize(Data);
            if(string.IsNullOrEmpty(data)) return;
            Debug.Log("[SAVE] Data Saved:" + 
                      data.Substring(0,Mathf.Min(data.Length,100)));
            PlayerPrefs.SetString(PlPrefsDataKey, data);
        }
        public static void ClearData()
        {
            _player = null;
            PlayerPrefs.DeleteKey(PlPrefsDataKey);
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
        }
        protected class PlayerData
        {
            public int lastCompleteLevelIndex = 0;
            public int lastStartedLevel = 1;
            public string customUserData = "";
        }
    }
}