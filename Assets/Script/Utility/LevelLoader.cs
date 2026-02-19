using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

public class LevelLoader : MonoBehaviourPun
{
    public static LevelLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region LOAD BY NAME
    public int lastPlayedLevelIndex = -1;

    public void LoadScene(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            photonView.RPC(nameof(RequestLoadScene), RpcTarget.MasterClient, sceneName);
        }
        
        FirebaseSessionLogger.Instance.AddLog("levelStarted: "+sceneName);
    }

    [PunRPC]
    private void RequestLoadScene(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void LoadLevel(int index)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(index);
        }
    }

    #endregion

    #region LOAD NEXT LEVEL

    public void LoadNextLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PerformLoadNext();
        }
        else
        {
            photonView.RPC(nameof(RequestLoadNext), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RequestLoadNext()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PerformLoadNext();
    }

    private void PerformLoadNext()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Prevent going out of bounds
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0; // or return;
        }

        PhotonNetwork.LoadLevel(nextIndex);
        FirebaseSessionLogger.Instance.AddLog("levelStarted: "+SceneManager.GetSceneByBuildIndex(nextIndex));
    }


    #endregion

    #region RESTART

    // public void RestartLevel()
    // {
        
    //     if(PhotonNetwork.IsMasterClient)
    //     {
    //         PerformRestart();
    //     }else
    //         photonView.RPC(nameof(RequestRestart), RpcTarget.MasterClient);
        
    // }

    // [PunRPC]
    // private void RequestRestart()
    // {
    //     if (!PhotonNetwork.IsMasterClient) return;
    //     PerformRestart();
    // }

    // private void PerformRestart()
    // {
    //     Debug.Log("Restart requested by: " + PhotonNetwork.NickName);
    //     Debug.Log("IsMaster: " + PhotonNetwork.IsMasterClient);
    //     Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);

    //     StartCoroutine(DelayedRestart());
    // }

    // private IEnumerator DelayedRestart()
    // {
    //     PhotonNetwork.AutomaticallySyncScene = true;
    //     yield return new WaitForSeconds(0.2f);

    //     int currentIndex = SceneManager.GetActiveScene().buildIndex;
    //     PhotonNetwork.LoadLevel(currentIndex);
    // }

    public void SaveThisLevelIndex()
    {
        lastPlayedLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_Restart), RpcTarget.All);
        }
        else
        {
            photonView.RPC(nameof(RequestRestart), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RequestRestart()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC(nameof(RPC_Restart), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Restart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            PhotonNetwork.LoadLevel("FakeLevel");
        }
    }

    #endregion
}
