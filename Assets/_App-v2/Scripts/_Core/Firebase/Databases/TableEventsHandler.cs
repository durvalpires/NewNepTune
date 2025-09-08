using System;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;
#if !UNITY_WEBGL
using Firebase.Database;
#endif
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Databases
{
    public class TableEventsHandler<T> : IDisposable where T : class, IDBData
    {
        #if !UNITY_WEBGL
        private readonly DatabaseReference _tableReference;
        #endif

        private readonly IDBTable<T> _table;
    
        #if !UNITY_WEBGL
        public TableEventsHandler(DatabaseReference tableReference, IDBTable<T> table)
        { 
            _tableReference = tableReference;
            _table = table;
            _tableReference.ValueChanged += HandleTableChange;
        }
        #endif

#if !UNITY_WEBGL
        private void HandleTableChange(object sender, ValueChangedEventArgs newData)
        {
            if (newData.DatabaseError != null)
            {
                Debug.LogError(newData.DatabaseError.Message);
                return;
            }
        
            var json = newData.Snapshot.GetRawJsonValue();
            var data = JsonUtility.FromJson<T>(json);
            _table.WriteData(data);
        }
#endif


        public void Dispose()
        {
            #if !UNITY_WEBGL
            if (_tableReference != null)
                _tableReference.ValueChanged -= HandleTableChange;
            #endif

        }
    }
}