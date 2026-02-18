using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FirebaseSessionLogger : MonoSingleton<FirebaseSessionLogger>
{
    [SerializeField] private SessionLogData currentSession;
    [SerializeField] private bool sendData = true;

    private bool sessionStarted = false;



    #region SESSION CONTROL

    public void StartSession()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        string sessionId = System.Guid.NewGuid().ToString();

        currentSession = new SessionLogData(
            sessionId,
            "empty",
            "empty"
        );

        sessionStarted = true;
    }

    public void SetPlayers(string player1, string player2)
    {
        if (currentSession == null) return;

        currentSession.player1 = player1;
        currentSession.player2 = player2;

    }

    public void AddLog(string message)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!sessionStarted || currentSession == null) return;

        string time = System.DateTime.UtcNow.ToString("HH:mm:ss");
        currentSession.logs.Add($"[{time}] {message}");
    }

    public void EndAndUploadSession()
    {
        if (!PhotonNetwork.IsMasterClient || !sendData) return;
        if (!sessionStarted || currentSession == null) return;

        currentSession.endTime =
            System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = JsonUtility.ToJson(currentSession);

        // 🚀 WebGL Upload
        WebglJavascriptBridge.Instance.UploadToJS(json);

        sessionStarted = false;
        currentSession = null;
    }

    public void SetCompletionTime(float time, bool publish)
    {
        if (currentSession == null) return;

        currentSession.completionTime = time;
        currentSession.publishScore = publish;
    }


    #endregion

    #region APP LIFECYCLE

    private void OnApplicationQuit()
    {
        EndAndUploadSession();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            //EndAndUploadSession();
        }
    }

    #endregion
}
