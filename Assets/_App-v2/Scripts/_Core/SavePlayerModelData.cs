using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SavePlayerModelData : MonoBehaviour
    {
        public static SavePlayerModelData Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            PlayerModelBase.SaveData();
        }
    }
