using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IPlayerDataService
{
    AccountDataDB AccountData { get; }

    UniTask LoadPlayerData();
    UniTask SavePlayerData();
    void UpdatePlayerNameInFirebase();
}