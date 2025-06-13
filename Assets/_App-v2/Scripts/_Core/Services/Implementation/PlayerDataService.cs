using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
#if !UNITY_WEBGL
using Firebase.Auth;
#endif

public class PlayerDataService : IPlayerDataService
{
    private readonly IDataPersistence _dataPersistence;

    public AccountDataDB AccountData { get; private set; }

    public PlayerDataService(IDataPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
        AccountData = new AccountDataDB();
    }

    public async UniTask LoadPlayerData()
    {
        AccountData = await _dataPersistence.LoadPlayerData();
    }

    public async UniTask SavePlayerData()
    {
        await _dataPersistence.SavePlayerData(AccountData);
    }

    public void UpdatePlayerNameInFirebase()
    {
#if !UNITY_WEBGL
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            string nameToSet = string.IsNullOrEmpty(currentUser.DisplayName) ? "Default Name" : currentUser.DisplayName;
            _dataPersistence.UpdatePlayerName(nameToSet).Forget();
        }
#endif
    }
}
