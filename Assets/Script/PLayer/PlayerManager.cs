using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System.Collections;

public class PlayerManager : MonoBehaviourPun, IDamageable
{
    [SerializeField] private RagdollSwitcher ragdollSwitcher;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [Header("Events")]
    public UnityEvent PlayerDied;

    [Header("Game Event")]
    [SerializeField] private GameEvent Event_OnPlayerDied;

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    #region DAMAGE

    public void TakeDamage(int amount)
    {
        // 🔥 Only Master processes health logic
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (isDead)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            photonView.RPC(nameof(RPC_Die), RpcTarget.All);
        }
    }

    #endregion

    #region DEATH

    [PunRPC]
    private void RPC_Die()
    {
        if (isDead) return;

        isDead = true;

        ragdollSwitcher?.EnableRagdoll();

        PlayerDied?.Invoke();

        if (Event_OnPlayerDied != null)
        {
            Event_OnPlayerDied.Raise(this, true);
        }

        
        CheckpointSystem.Instance.RespawnPlayer(this.transform);
        
    }

    #endregion

    #region RESPAWN

    public void ListnToOnPlayerRespawn(Component sender,object  data)
    {
        photonView.RPC(nameof(RPC_Respawn),RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;

        ragdollSwitcher?.DisableRagdoll();
        AudioManager.instance.PlaySound("Respawn");

       
    }

    #endregion
}
