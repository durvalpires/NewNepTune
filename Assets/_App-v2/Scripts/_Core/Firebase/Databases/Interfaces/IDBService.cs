using Cysharp.Threading.Tasks;

namespace _App_v2.Scripts._Core.Firebase.Databases.Interfaces
{
    public interface IDBService
    {
        bool IsConnected { get; }
        void Connect(string userId, string subUser = null);
        void Disconnect();
        void SaveData<T>(T data) where T : class, IDBData, new();
        UniTask<T> GetData<T>() where T : class, IDBData, new();
    }
}