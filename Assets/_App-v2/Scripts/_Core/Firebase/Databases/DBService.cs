using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;
using Cysharp.Threading.Tasks;
#if !UNITY_WEBGL
using Firebase.Database;
#endif
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Databases
{
    public class DBService : IDBService
    {
        private const string UsersTable = "users";
        private const string StudentsTable = "students";
        
        private bool _isConnected;
        #if !UNITY_WEBGL
        private DatabaseReference _database;
        #endif

        private DBTablesManager _tables;

        bool IDBService.IsConnected => _isConnected;

        void IDBService.Connect(string teacherId, string studentId)
        {
            if(string.IsNullOrEmpty(teacherId))
            {
                Debug.LogError("User ID is null or empty");
                return;
            }

            #if !UNITY_WEBGL
            if (string.IsNullOrEmpty(studentId)) // Teacher's account
            {
                _database = FirebaseDatabase.DefaultInstance.RootReference.Child(UsersTable).Child(teacherId);
            }
            else // Student's account
            {
                _database = FirebaseDatabase.DefaultInstance.RootReference.Child(UsersTable)
                    .Child(teacherId)
                    .Child(StudentsTable)
                    .Child(studentId);
            }

            _database.ValueChanged += HandleDBChange;
            #endif

        }

        void IDBService.Disconnect()
        {
            _tables?.Dispose();
            
            #if !UNITY_WEBGL
            _database = null;
            #endif

            _tables = null;
            
            _isConnected = false;
            Debug.Log("DBService disconnected");
        }

        public void SaveData<T>(T data)
             where T : class, IDBData, new()
        {
            if (!_isConnected)
            {
                Debug.LogError("DBService is not connected");
                return;
            }
            _tables.OverrideData(data);
        }

        public async UniTask<T> GetData<T>()
            where T : class, IDBData, new()
        {
            if (!_isConnected)
            {
                Debug.LogError("DBService is not connected");
                return new T();
            }
            return await _tables.GetData<T>();
        }

#if !UNITY_WEBGL
        private void HandleDBChange(object sender, ValueChangedEventArgs valueChangedEventArgs)
        {
            _database.ValueChanged -= HandleDBChange;

            if (valueChangedEventArgs.DatabaseError != null)
            {
                Debug.LogError(valueChangedEventArgs.DatabaseError.Message);
                return;
            }

            _tables = new DBTablesManager(_database);
            _isConnected = true;
            
            Debug.Log($"User {_database.Key} connected to database");
        }
#endif

    }
}
