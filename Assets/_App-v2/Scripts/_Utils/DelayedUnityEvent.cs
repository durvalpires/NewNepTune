using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedUnityEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEvent;
    [SerializeField] private float delay;
    [SerializeField] bool doOnStart;
    [SerializeField] bool doOnEnable;
    [SerializeField] bool doOnDisable;
    [SerializeField] private string comment;
    void Start()
    {
        if (doOnStart)
            StartDelay();
    }
    void OnEnable()
    {
        if (doOnEnable)
            StartDelay();
    }

    private void OnDisable()
    {
        if (doOnDisable)
        {
            delay.Delay(Do);
        }
    }

    public void DoEvent()
    {
        StartDelay();
    }
    private void StartDelay()
    {
        Invoke("Do", delay);

    }

    private void Do()
    {
        OnEvent.Invoke();
    }
}
