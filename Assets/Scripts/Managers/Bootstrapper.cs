using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public static class Bootstrapper
    {
        // #if !UNITY_WEBGL
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // public static void Execute() => Object.DontDestroyOnLoad(Addressables.InstantiateAsync("Systems").WaitForCompletion());
        // #else
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // public static async void Execute()
        // {
        //     var handle = Addressables.InstantiateAsync("Systems");
        //     var obj = await handle.Task; // Asynchronous loading
        //     Object.DontDestroyOnLoad(obj);
        // }
        // #endif
    }
}