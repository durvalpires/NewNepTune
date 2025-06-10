using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MainMenuInitController : MonoBehaviour
{
    [SerializeField] private UnityEvent onStart;
    private void Start()
    {
        var locations = new List<IResourceLocation>();
        Addressables.LoadResourceLocationsAsync("").Completed += handle => {
            foreach (var loc in handle.Result)
                Debug.Log($"Addressables Key: {loc.PrimaryKey}");
        };
        onStart.Invoke();
    }
}
