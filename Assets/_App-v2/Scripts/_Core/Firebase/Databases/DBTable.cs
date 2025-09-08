using System;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;
#if !UNITY_WEBGL
using Firebase.Database;
#endif
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Databases
{
    /// <summary>
    /// Represents JSON Object in the DB
    /// </summary>
    /// <typeparam name="T">Data type that represents the project data object converted from a DB JSON object</typeparam>
    public class DBTable<T> : IDBTable<T> where T : class, IDBData, new()
    {
        private readonly string _tableName;
        private TableEventsHandler<T> _tableEventsHandler;
        private T _data;

        string IDBTable.Name => _tableName;

        private event Action<T> OnDataChanged;

        event Action<T> IDBTable<T>.OnDataChanged
        {
            add => this.OnDataChanged += value;
            remove => this.OnDataChanged -= value;
        }

        #if !UNITY_WEBGL
        public DBTable(string tableName, DatabaseReference reference)
        {
            _tableName = tableName;
            _tableEventsHandler = new TableEventsHandler<T>(reference, this);
            _data = new T();
        }
        public DBTable(DataSnapshot snapshot)
        {
            _tableEventsHandler = new TableEventsHandler<T>(snapshot.Reference, this);
        
            _tableName = snapshot.Key;
            var json = snapshot.GetRawJsonValue();
            _data = JsonUtility.FromJson<T>(json);
        }
        #endif


        void IDBTable<T>.WriteData(T data)
        {
            _data = data;
            OnDataChanged?.Invoke(_data);
        }

        T IDBTable<T>.ReadData() => _data;

        void IDisposable.Dispose()
        {
            _tableEventsHandler?.Dispose();
            OnDataChanged = null;
            _data = null;
        }
    }
}