using System.Collections.Generic;
using _App_v2.Scripts._Core.Firebase;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if !UNITY_WEBGL
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
#endif

public class FirebaseDataPersistence : IDataPersistence
{
    public bool IsConnected => FireService.Instance.DB.IsConnected;

#if !UNITY_WEBGL
    private DatabaseReference GetUserDbRef()
    {
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser == null)
            return null;
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUser.UserId);
    }
#endif

    public async UniTask<AccountDataDB> LoadPlayerData()
    {
#if !UNITY_WEBGL
        var userRef = GetUserDbRef();
        if (userRef == null)
            return new PlayerDataDB();

        DataSnapshot playerDataSnapshot = await userRef.Child("PlayerData").GetValueAsync();
        if (playerDataSnapshot.Exists)
        {
            string json = playerDataSnapshot.GetRawJsonValue();
            return JsonUtility.FromJson<PlayerDataDB>(json);
        }
#endif
        return new AccountDataDB();
    }

    public async UniTask SavePlayerData(AccountDataDB accountData)
    {
#if !UNITY_WEBGL
        var dbRef = GetUserDbRef()?.Child("PlayerData");
        if (dbRef == null) return;

        await dbRef.Child("profileId").SetValueAsync(playerData.profileId);
        await dbRef.Child("playerName").SetValueAsync(playerData.playerName);
        await dbRef.Child("IsSoundOn").SetValueAsync(playerData.IsSoundOn);
        await dbRef.Child("IsMusicOn").SetValueAsync(playerData.IsMusicOn);
        await dbRef.Child("customUserData").SetValueAsync(playerData.customUserData);
#endif
    }

    public async UniTask<PlayerLevelData> LoadLevelData(string subProfileName)
    {
#if !UNITY_WEBGL
        var userRef = GetUserDbRef();
        if (userRef == null)
            return new PlayerLevelData { SubProfilename = subProfileName };

        var levelDataSnapshot = await userRef.Child("SubProfile")
                                         .Child(subProfileName)
                                         .Child("playerLeveldata")
                                         .GetValueAsync();
        if (levelDataSnapshot.Exists)
        {
            string json = levelDataSnapshot.GetRawJsonValue();
            var container = JsonUtility.FromJson<PlayerLevelData>(json);
            if (container != null)
            {
                return container;
            }
        }
#endif
        return new PlayerLevelData { SubProfilename = subProfileName };
    }

    public async UniTask SaveLevelData(PlayerLevelData levelData, string subProfileName)
    {
#if !UNITY_WEBGL
        var dbRef = GetUserDbRef()
            ?.Child("SubProfile")
            .Child(subProfileName)
            .Child("playerLeveldata");

        if (dbRef == null) return;

        await dbRef.Child("SubProfilename").SetValueAsync(levelData.SubProfilename);
        await dbRef.Child("AverageScore").SetValueAsync(levelData.AverageScore);
        await dbRef.Child("NumberOfLevel").SetValueAsync(levelData.NumberOfLevel);
        await dbRef.Child("Repetition").SetValueAsync(levelData.Repetition);
        await dbRef.Child("customLevelData").SetValueAsync(levelData.customLevelData);

        var levelsRef = dbRef.Child("levels");
        foreach (var entry in levelData.levels)
        {
            var levelRef = levelsRef.Child(entry.Key.ToString());
            await levelRef.Child("Score").SetValueAsync(entry.Value.Score);
            await levelRef.Child("Repetition").SetValueAsync(entry.Value.Repetition);
            await levelRef.Child("Attempts").SetValueAsync(entry.Value.Attempts);
            await levelRef.Child("Success").SetValueAsync(entry.Value.Success);
            await levelRef.Child("Failure").SetValueAsync(entry.Value.Failure);
            await levelRef.Child("TimeSpent").SetValueAsync(entry.Value.TimeSpent);
        }
#endif
    }

    public async UniTask UpdatePlayerName(string playerName)
    {
#if !UNITY_WEBGL
        var dbRef = GetUserDbRef()?.Child("PlayerData");
        if (dbRef != null)
        {
            await dbRef.Child("playerName").SetValueAsync(playerName);
        }
#endif
    }

    public async UniTask UpdateLevelCounter(int levelIndex, CounterType counterType, int newValue, string subProfileName)
    {
#if !UNITY_WEBGL
        var dbRef = GetUserDbRef()
            ?.Child("SubProfile")
            ?.Child(subProfileName)
            ?.Child("playerLeveldata")
            ?.Child("levels")
            ?.Child(levelIndex.ToString());

        if (dbRef != null)
        {
            await dbRef.Child(counterType.ToString()).SetValueAsync(newValue);
        }
#endif
    }

    public async UniTask CreateSubProfile(string subProfileName, PlayerLevelData initialData)
    {
#if !UNITY_WEBGL
        var userRef = GetUserDbRef();
        if (userRef == null) return;

        var subProfileRef = userRef.Child("SubProfile").Child(subProfileName);
        var snapshot = await subProfileRef.GetValueAsync();
        if (!snapshot.Exists)
        {
            string json = JsonUtility.ToJson(initialData);
            await subProfileRef.Child("playerLeveldata").SetRawJsonValueAsync(json);
        }
#endif
    }

    public async UniTask<List<string>> GetAllSubProfiles()
    {
        List<string> subProfileNames = new List<string>();

#if !UNITY_WEBGL
        var userRef = GetUserDbRef();
        if (userRef != null)
        {
            var snapshot = await userRef.Child("SubProfile").GetValueAsync();
            if (snapshot.Exists)
            {
                foreach (var child in snapshot.Children)
                {
                    subProfileNames.Add(child.Key);
                }
            }
        }
#endif

        return subProfileNames;
    }
}