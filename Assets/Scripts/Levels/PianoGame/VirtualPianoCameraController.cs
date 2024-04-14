using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPianoCameraController : MonoBehaviour
{
    private Transform target;

    private bool oneTime = false;

    private PianoGameManagerFinal _pianoGameManagerFinal;

    private void Start()
    {
        _pianoGameManagerFinal = FindObjectOfType<PianoGameManagerFinal>();
    }
    
    private void Update()
    {
        if (!oneTime && _pianoGameManagerFinal.isGameStarted && target == null)
        {
            findTarget();
            oneTime = true;
        }

        if (target != null)
        {
            gameObject.transform.position = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        }
    }
    
    
    private void findTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
