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
    protected double networkStartTime;
    protected double stunStartNetworkTime;
    protected double pausedElapsedTime;



    protected virtual void Awake()
    {
        

        if (stunnedParticle != null)
            stunnedParticle.Stop();
    }

    private void Update()
    {
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

        double stunTime = PhotonNetwork.Time;

        photonView.RPC(nameof(RPC_StunObstacle), RpcTarget.All, stunTime, duration);
    }


    [PunRPC]
    public void RPC_StunObstacle(double networkTime, float duration)
    {
        double lag = PhotonNetwork.Time - networkTime;

        ApplyStun(duration, lag);
    }


    

    private void ApplyStun(float duration, double lag)
    {
        isStunned = true;

        stunTimer = duration - (float)lag;

        stunStartNetworkTime = PhotonNetwork.Time;
        pausedElapsedTime = PhotonNetwork.Time - networkStartTime;


        if (stunTimer < 0)
            stunTimer = 0;

        if (stunnedParticle != null)
            stunnedParticle.Play();

        stunedFeedback?.PlayFeedback();

        Update_Del -= ActiveUpdate;
        Update_Del += HandleStun;
    }

    private void HandleStun()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0f)
        {
            Update_Del -= HandleStun;
            Update_Del += ActiveUpdate;

            isStunned = false;

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RPC_EndStunVisual), RpcTarget.All);
            }
        }
    }


    [PunRPC]
    public void RPC_EndStunVisual()
    {
        if (stunnedParticle != null)
            stunnedParticle.Stop();
        networkStartTime = PhotonNetwork.Time - pausedElapsedTime;

    }



    #endregion

    public void TakeDamage(int amount)
    {
        OnShot(0);
    }

    public void ListnToOnInitEvent(Component sender,object data)
    {
        if((bool)data)
        {
            networkStartTime = PhotonNetwork.Time;
            Update_Del += ActiveUpdate;
        }
    }
}
