using System;

namespace _App_v2.Scripts._Core.Firebase.Databases.Interfaces
{
    public interface IDBTable : IDisposable
    {
        string Name { get; }
    }
    
    public interface IDBTable<T> : IDBTable where T : class, IDBData
    {
        event Action<T> OnDataChanged;
        void WriteData(T data);
        T ReadData();
    }
}