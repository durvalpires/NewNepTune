using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Antari
{
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

        public static int LastStartedLevel
        {
            get => Data.lastStartedLevel;
            set => Data.lastStartedLevel = value;
        }
        public static int LastCompleteLevelIndex
        {
            get => Data.lastCompleteLevelIndex;
            set => Data.lastCompleteLevelIndex = value;
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
}
