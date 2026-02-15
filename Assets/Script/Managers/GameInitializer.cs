using UnityEngine;
using Photon.Pun;

public class GameInitializer : MonoBehaviourPun
{
    [Header("Init Settings")]
    [SerializeField] private float delayBeforeInit = 3f;

    [Header("Game Event")]
    [SerializeField] private GameEvent event_Initialize;

    private bool initialized = false;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Invoke(nameof(SendInitializeRPC), delayBeforeInit);
    }

    private void SendInitializeRPC()
    {
        if (initialized) return;

        photonView.RPC(nameof(RPC_DoInitialize), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_DoInitialize()
    {
        if (initialized) return;

        initialized = true;

        if (event_Initialize != null)
        {
            event_Initialize.Raise(this, true);
        }

        Debug.Log("Game Initialized on: " + PhotonNetwork.LocalPlayer.NickName);
    }                         
}
