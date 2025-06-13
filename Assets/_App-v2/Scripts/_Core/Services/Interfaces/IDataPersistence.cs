using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IDataPersistence
{
    bool IsConnected { get; }

    UniTask<AccountDataDB> LoadPlayerData();
    UniTask SavePlayerData(AccountDataDB accountData);

    UniTask<PlayerLevelData> LoadLevelData(string subProfileName);
    UniTask SaveLevelData(PlayerLevelData levelData, string subProfileName);

    UniTask UpdatePlayerName(string playerName);
    UniTask UpdateLevelCounter(int levelIndex, CounterType counterType, int newValue, string subProfileName);
    UniTask CreateSubProfile(string subProfileName, PlayerLevelData initialData);
    UniTask<List<string>> GetAllSubProfiles();
}