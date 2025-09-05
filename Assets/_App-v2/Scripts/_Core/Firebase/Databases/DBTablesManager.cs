using System;
using System.Collections.Generic;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;
using Cysharp.Threading.Tasks;
#if !UNITY_WEBGL
using Firebase.Database;
#endif
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Databases
{
    public class DBTablesManager : IDisposable
    {
        #if !UNITY_WEBGL
        private readonly DatabaseReference _database;
        #endif

        private readonly Dictionary<Type, IDBTable> _tables = new();

        #if !UNITY_WEBGL
        public DBTablesManager(DatabaseReference database)
        {
            _database = database;
        }
#endif
        
        public async UniTask<IDBTable<T>> InitializeTable<T>() where T : class, IDBData, new()
        {
            var type = typeof(T);
            if (_tables.ContainsKey(type))
            {
                Debug.LogError($"Table with name {type.Name} already exists");
                return null;
            }
            
            IDBTable<T> table = null;
            
#if !UNITY_WEBGL
            var snapshot = await _database.Child(type.Name).GetValueAsync().AsUniTask();

            if (snapshot == null || !snapshot.Exists)
            {
                table = new DBTable<T>(type.Name, _database);
            }
            else
            {
                table = new DBTable<T>(snapshot);
            }
            
            _tables.Add(type, table);
#endif
            return table;
        }
        
        public async UniTask<T> GetData<T>() where T : class, IDBData, new()
        {
            if (!TryGetTable(out IDBTable<T> table))
                table = await InitializeTable<T>();
            
            return table.ReadData();
        }
        
        public async void OverrideData<T>(T data, bool repeat = false) where T : class, IDBData, new()
        {
            if (data == null)
            {
                Debug.LogWarning("Data is null. Skipping override");
                return;
            }
            
            var tableName = typeof(T).Name;
            
            if (TryGetTable(out IDBTable<T> table))
            {
                table.WriteData(data);
                UpdateRemoteTable(table);
            }
            else if (!repeat)
            { 
                await InitializeTable<T>();
                OverrideData(data, true);
            }
            else
            {
                Debug.LogError($"Failed to override data for table {tableName}");
            }
        }
        
        public void RemoveTable<T>() where T : class, IDBData
        {
            if (!TryGetTable(out IDBTable<T> table))
                return;
         
            var type = typeof(T);
            table.Dispose();
            _tables.Remove(type);
            #if !UNITY_WEBGL
            _database.Child(table.Name).RemoveValueAsync();
            #endif

        }

        public void Dispose()
        {
            ClearTables();
        }
        
        private bool TryGetTable<T>(out IDBTable<T> table) where T : class, IDBData
        {
            var type = typeof(T);
            table = null;

            if (_tables.TryGetValue(type, out var dbTable))
            {
                table = dbTable as IDBTable<T>;
            }

            return table != null;
        }
        
        private void UpdateRemoteTable<T>(IDBTable<T> table) where T : class, IDBData
        {
            var json = JsonUtility.ToJson(table.ReadData());
            #if !UNITY_WEBGL
            _database.Child(table.Name).SetRawJsonValueAsync(json);
            #endif

        }

        private void ClearTables()
        {
            foreach (var table in _tables)
            {
                if (table.Value is IDisposable disposable)
                    disposable.Dispose();
            }
            _tables.Clear();
        }
    }
}