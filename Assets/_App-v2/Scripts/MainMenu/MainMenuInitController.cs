using System;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuInitController : MonoBehaviour
{
    [SerializeField] private UnityEvent onStart;
    private void Start()
    {
        onStart.Invoke();
    }
}
