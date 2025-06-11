using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class PlayerModel : PlayerModelBase
    {
        public static event Action onCurrencyChanged;
        private static IResourceProvider _resourceProvider;

        // public static float AudioVolume
        // {
        //     get => (float)Data.audioVolume;
        //     set
        //     {
        //         Data.audioVolume = value;
        //     }
        // }

        /*
        private static List<string> _levelProgress;// = new List<string>();
        public static List<string> LevelProgress
    {
        get
        {
            if (Data.levelProgress != "")
            {
                _levelProgress = Data.levelProgress.Split(',').ToList();
            }
            else
            {
                _levelProgress = new List<string>();
            }
            return _levelProgress;
        }
    }
    public static void AddItemLevelProgress(int index)
    {
        if(_levelProgress.Contains(index.ToString())) return;

        _levelProgress.Add(index.ToString());
        Data.levelProgress = _levelProgress.Count > 0 ? string.Join(",", _levelProgress) : "";
    }

    public static void ResetLevelProgress()
    {
        Data.levelProgress = "";
    }
    */

    // public static int CompletedWorldIndex
    // {
    //     get => Data.lastCompletedWorldIndex;
    //     set => Data.lastCompletedWorldIndex = value;
    // }

    // public static int LastCompleteLevelIndex
    // {
    //     get => Data.lastCompleteLevelIndex;
    //     set => Data.lastCompleteLevelIndex = value;
    // }

        private async void Awake()
        {
            await PlayerModel.LoadData();
        }
        public static void CompleteLevel(int levelIndex, string worldId)
        {
            SetCustomData($"w_{worldId}:l_{levelIndex}", "done");
            SetLevelCompleted(levelIndex);
        }

        public static bool IsLevelCompleted(int levelIndex, string worldId)
        {
            return GetCustomData($"w_{worldId}:l_{levelIndex}") == "done";
        }
        public static void CompleteWorld(string worldId)
        {
            SetCustomData($"w_{worldId}", "done");
        }

        private static AllWorldsSO _allWorlds;
        public static AllWorldsSO AllWorlds
        {
            get
            {
                if (_allWorlds == null)
                    _allWorlds = Resources.Load<AllWorldsSO>("AllWorldsSO");

                return _allWorlds;
            }
        }
        private static GeneralConfigSO _generalConfig;
        public static GeneralConfigSO GeneralConfig
        {
            get
            {
                if (_generalConfig == null)
                    _generalConfig = Resources.Load<GeneralConfigSO>("GeneralConfigSO");

                return _generalConfig;
            }
        }
        public static bool IsWorldCompleted(string worldId)
        {
            return GetCustomData($"w_{worldId}") == "done";
        }
      
        
        public static IResourceProvider ResourceProvider
        {
            get
            {
                if (_resourceProvider == null)
                    _resourceProvider = new ResourcesLoader();
                return _resourceProvider;
            }
        }
    }
