using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class AddressablesInitializedEvent : UnityEvent<bool> { }

public class AddressablesBootstrap : MonoBehaviour
{
    [Header("Events")]
    public AddressablesInitializedEvent onAddressablesInitialized = new AddressablesInitializedEvent();

    private async void Start()
    {
        Debug.Log("[Bootstrap] Initializing Addressables...");
        Debug.Log("[Bootstrap] StreamingAssets path: " + Application.streamingAssetsPath);
        
        // // Print out catalog paths BEFORE initialization
        // var locator = Addressables.ResourceLocators;
        // if (locator != null)
        //     Debug.Log($"[Bootstrap] Existing locators: {locator.Count()}");
        //
        // // If Addressables already has default settings, show where it's looking
        // if (Addressables.ResourceLocators != null)
        // {
        //     foreach (var l in Addressables.ResourceLocators)
        //         Debug.Log($"[Bootstrap] Locator: {l}");
        // }

        // Initialize Addressables
        var initHandle = Addressables.InitializeAsync();
        await initHandle.Task;

        Debug.Log($"Addressables initialized. Locators: {Addressables.ResourceLocators.Count()}");

        if (Addressables.ResourceLocators.Count() == 0)
        {
            Debug.LogError($"No Addressables locators found. Check that bundles are in {Application.streamingAssetsPath}/aa/WebGL!");
            return;
        }

        // // Optional: List all keys in the first locator for debug
        // var locator = Addressables.ResourceLocators.First();
        // foreach (var key in locator.Keys)
        // {
        //     Debug.Log($"Locator key: {key}");
        // }
        
        Debug.Log("Init Handle: " + initHandle.Status);
        
        try
        {
            //await initHandle.Task;
            
        
            if (initHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("[Bootstrap] Addressables initialized successfully!");
                onAddressablesInitialized?.Invoke(true);
            }
            else
            {
                Debug.LogError("[Bootstrap] Addressables initialization failed!");
                onAddressablesInitialized?.Invoke(false);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Bootstrap] Exception during initialization: {ex}");
            onAddressablesInitialized?.Invoke(false);
        }
        finally
        {
            await System.Threading.Tasks.Task.Yield(); // ensure next frame
            Debug.Log("[Bootstrap] Finally.");
            // if (initHandle.IsValid())
            //     Addressables.Release(initHandle);
        }
        
        onAddressablesInitialized?.Invoke(true);
    }
}