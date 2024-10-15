using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseSaveSystem : MonoBehaviour
{
    DatabaseReference databaseReference;

    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SavePlayerData(PlayerProfile profile, ProgressionData progression)
    {
        string jsonProfile = JsonUtility.ToJson(profile);
        string jsonProgression = JsonUtility.ToJson(progression);

        // Save player profile and progression data
        databaseReference.Child("users").Child(profile.playerName).Child("profile").SetRawJsonValueAsync(jsonProfile);
        databaseReference.Child("users").Child(profile.playerName).Child("progression").SetRawJsonValueAsync(jsonProgression);
    }

    public void LoadPlayerData(string playerName, System.Action<PlayerProfile, ProgressionData> onComplete)
    {
        databaseReference.Child("users").Child(playerName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                PlayerProfile profile = JsonUtility.FromJson<PlayerProfile>(snapshot.Child("profile").GetRawJsonValue());
                ProgressionData progression = JsonUtility.FromJson<ProgressionData>(snapshot.Child("progression").GetRawJsonValue());

                onComplete(profile, progression); // Callback with the loaded data
            }
        });
    }
}