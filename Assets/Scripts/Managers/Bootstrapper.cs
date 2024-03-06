using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() => Object.DontDestroyOnLoad(Addressables.InstantiateAsync("Systems").WaitForCompletion());
    }
}