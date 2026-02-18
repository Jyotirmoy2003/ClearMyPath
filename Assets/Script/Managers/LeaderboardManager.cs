using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class LeaderboardManager : MonoBehaviour
{

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void FetchLeaderboard();
#endif

    public void RequestLeaderboard()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FetchLeaderboard();
#endif
    }


    void Start()
    {
        RequestLeaderboard();
    }



    [SerializeField] Transform leaderBoardDataContainer;
    [SerializeField] LeaderBoardLable leaderBoardLablePrefab;



    public void OnLeaderboardDataReceived(string json)
    {
        List<SessionLogData> sessions = JsonHelper.FromJson<SessionLogData>(json);

        sessions = sessions.FindAll(s => s.publishScore);

        sessions.Sort((a, b) => a.completionTime.CompareTo(b.completionTime));

        DisplayLeaderboard(sessions);
    }

    private void DisplayLeaderboard(List<SessionLogData> sessions)
    {
        // Clear previous data
        for (int i = leaderBoardDataContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(leaderBoardDataContainer.GetChild(i).gameObject);
        }


        int rank = 1;

        foreach (var session in sessions)
        {
            Instantiate(leaderBoardLablePrefab,leaderBoardDataContainer).SetData(rank,session.player1,session.player2,session.completionTime);

            rank++;

            if (rank > 10) break;
        }
    }

    public void RefetchLeaderboard()
    {
        
        RequestLeaderboard();
    }
}
