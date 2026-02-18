using UnityEngine;
using Photon.Pun;

public class BombPouch : MonoBehaviourPun
{
    [SerializeField] private int bombAmount = 3;
    [SerializeField] ParticleSystem collectedParticle;
    [SerializeField] GameObject bombMesh;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (!other.CompareTag("Player"))
            return;

        // Only female should trigger it (non-master)
        if (PhotonNetwork.IsMasterClient)
            return;

        collected = true;

        // Tell master that female collected pouch
        photonView.RPC(nameof(RPC_RequestCollect), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_RequestCollect(PhotonMessageInfo info)
    {
        // Only master executes this
        if (!PhotonNetwork.IsMasterClient)
            return;

        

        // Notify all clients to destroy pouch + show UI
        photonView.RPC(nameof(RPC_ConfirmCollect), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ConfirmCollect()
    {
        // Optional: Play particle or UI effect here
        UIManager.Instance.ShowBombAddedUI(bombAmount);
        // Give bombs to master player
        ActionManager.AC_BombCollected?.Invoke(bombAmount);
        AudioManager.instance.PlaySound("Collect");

        collectedParticle.Play();

        bombMesh.SetActive(false);
        Destroy(gameObject,2f); //wait for particle to finish
    }
}
