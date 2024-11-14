using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SelfDestroy : MonoBehaviour
    {
        public float delay;
        void Start()
        {
            delay.Delay(() =>
            {
                if (this == null) return;
                    Destroy(gameObject);
            });
        }

    }
