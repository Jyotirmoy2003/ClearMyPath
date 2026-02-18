using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System.Collections;

public class PlayerManager : MonoBehaviourPun, IDamageable
{
    [SerializeField] PhotonTransformView ptv;
    [SerializeField] private RagdollSwitcher ragdollSwitcher;
    [SerializeField] CharacterController characterController;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [Header("Events")]
    public UnityEvent PlayerDied;

    [Header("Game Event")]
    [SerializeField] private GameEvent Event_OnPlayerDied;

    [SerializeField]private bool isDead = false;
    [SerializeField]private bool isImmune = false;

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

        if (isDead || isImmune)
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
        //ptv.enabled = false;
        characterController.enabled = false;
        isDead = true;

        ragdollSwitcher?.EnableRagdoll();

        PlayerDied?.Invoke();

        isImmune = true;
        if (Event_OnPlayerDied != null)
        {
            Event_OnPlayerDied.Raise(this, true);
        }

        if(!PhotonNetwork.IsMasterClient)
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
        
        currentHealth = maxHealth;

        ragdollSwitcher?.DisableRagdoll();
        AudioManager.instance.PlaySound("Respawn");
        //ptv.enabled = true;
        Invoke(nameof(EnableComponents),1f);
       
    }

    void EnableComponents()
    {
        isImmune = false;
        isDead = false;
        if(!PhotonNetwork.IsMasterClient)
        {
            characterController.enabled = true;
        }
        
    }

    #endregion
}
