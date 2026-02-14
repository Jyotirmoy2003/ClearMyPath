using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

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
    }

    [PunRPC]
    private void RequestLoadScene(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel(sceneName);
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
    }

    #endregion

    #region RESTART

    public void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PerformRestart();
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
        PerformRestart();
    }

    private void PerformRestart()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        PhotonNetwork.LoadLevel(currentIndex);
    }

    #endregion
}
