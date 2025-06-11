using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IPlayerDataService
{
    PlayerDataDB PlayerData { get; }

    UniTask LoadPlayerData();
    UniTask SavePlayerData();
    void UpdatePlayerNameInFirebase();
    string GetCustomData(string key, string defaultValue = "");
    void SetCustomData(string key, string value);
    Dictionary<string, object> GetAllCustomData();
}