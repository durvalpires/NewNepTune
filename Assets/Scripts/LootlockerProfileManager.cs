using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker;
using LootLocker.Requests;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class LootlockerProfileManager : MonoBehaviour
{
    public TextMeshProUGUI getPlayerText;
    public TextMeshProUGUI getPlayerFirstLevelScore;
    public LootLockerLeaderboard lb;
    [Header("Leaderboard Texts")]
    public TextMeshProUGUI playername;
    public TextMeshProUGUI playerscore;
    void Start()
    {
        StartCoroutine(FetchTopHighscoresRoutine());
        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                getPlayerText.text = "Welcome back, " + response.name;

            } else
            {
                Debug.Log("Error getting player name");
            }
        });
    }
    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        string leaderboardKey = "first_game_score_highscore";
        LootLockerSDKManager.GetScoreList(leaderboardKey, 5, 0, (response) =>
        {
            if(response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";
                LootLockerLeaderboardMember[] members = response.items;
                for(int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if(members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id; //isim yoksa id ye göre sırala
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playername.text = tempPlayerNames;
                playerscore.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public void GoToDikeySahne()
    {
        try
        {
            //SceneManager.LoadScene("DikeySahne");
            SceneManager.LoadScene("LevelSelectionSceneUpdated");
        }
        catch (System.Exception ex)
        {
            // Hata durumunda konsola bir hata mesajı yaz
            Debug.LogError("Hata yakalandı: " + ex.Message);
        }
    }
}
