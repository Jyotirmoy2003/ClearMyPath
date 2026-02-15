using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FirebaseSessionLogger : MonoSingleton<FirebaseSessionLogger>
{
    [SerializeField] private SessionLogData currentSession;
    [SerializeField] private bool sendData = true;

    private bool sessionStarted = false;

    #region WEBGL BRIDGE

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadSession(string json);
#endif

    private void UploadToJS(string json)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UploadSession(json);
        Debug.Log("Session sent to JS Firebase.");
#endif
    }

    #endregion

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

        AddLog("Both players connected. Session started.");
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
        UploadToJS(json);

        sessionStarted = false;
        currentSession = null;
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
            EndAndUploadSession();
        }
    }

    #endregion
}
