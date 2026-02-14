using UnityEngine;
using Photon.Pun;
using System;

public abstract class ObstacleBase : MonoBehaviourPun, IObstacle, IDamageable
{
    [Header("Damage Settings")]
    [SerializeField] protected int damageAmount = 1;
    [SerializeField] protected float damageCooldown = 1f;

    [Header("Stun Settings")]
    [SerializeField] protected float defaultStunDuration = 2f;
    [SerializeField] protected ParticleSystem stunnedParticle;
    [SerializeField] protected FeedBackManager stunedFeedback;

    protected Action Update_Del;

    private float lastDamageTime = -Mathf.Infinity;
    private bool isStunned;
    private float stunTimer;

    protected virtual void Awake()
    {
        Update_Del = ActiveUpdate;

        if (stunnedParticle != null)
            stunnedParticle.Stop();
    }

    private void Update()
    {
        // Only Master simulates movement & timer
        if (!PhotonNetwork.IsMasterClient)
            return;

        Update_Del?.Invoke();
    }

    protected abstract void ActiveUpdate();

    #region DAMAGE

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        TryDamage(collision.collider);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        TryDamage(other);
    }

    protected virtual void TryDamage(Collider other)
    {
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }

    #endregion

    #region STUN

    public virtual void OnShot(float stunDuration)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (isStunned)
            return;

        float duration = stunDuration > 0 ? stunDuration : defaultStunDuration;

        // Tell everyone visually
        photonView.RPC(nameof(RPC_StartStunVisual), RpcTarget.All);

        // Master handles timer
        isStunned = true;
        stunTimer = duration;

        Update_Del -= ActiveUpdate;
        Update_Del += HandleStun;
    }

    [PunRPC]
    public void RPC_StartStunVisual()
    {
        if (stunnedParticle != null)
            stunnedParticle.Play();

        stunedFeedback?.PlayFeedback();
    }

    private void HandleStun()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0f)
        {
            photonView.RPC(nameof(RPC_EndStunVisual), RpcTarget.All);

            isStunned = false;

            Update_Del += ActiveUpdate;
            Update_Del -= HandleStun;
        }
    }

    [PunRPC]
    public void RPC_EndStunVisual()
    {
        if (stunnedParticle != null)
            stunnedParticle.Stop();
    }

    #endregion

    public void TakeDamage(int amount)
    {
        OnShot(0);
    }
}
