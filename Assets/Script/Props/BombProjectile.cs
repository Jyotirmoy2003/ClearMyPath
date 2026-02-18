using UnityEngine;
using Photon.Pun;
using System;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] ParticleSystem exlosionEffect;
    [SerializeField] AudioSource exlosionAudio;
    [SerializeField] GameObject bombMesh;

    private ObjectPool pool;
    private float timer;
    private bool isAuthoritative;
    private Action updateDel;

    public void Initialize(ObjectPool objectPool, bool authoritative)
    {
        pool = objectPool;
        isAuthoritative = authoritative;
        timer = lifetime;
        bombMesh.SetActive(true);
        updateDel += MoveForward;
        Invoke(nameof(ReturnToPool),lifetime);
    }

    private void Update()=>updateDel?.Invoke();

    void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

       
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collsion with: "+other.name +"bool val:"+isAuthoritative);
        if (isAuthoritative && PhotonNetwork.IsMasterClient)
        {
            IObstacle obstacle = other.GetComponent<IObstacle>();
            if (obstacle != null)
            {
                obstacle.OnShot(stunDuration);
            }
        }

        updateDel -= MoveForward;
        bombMesh.SetActive(false);
        
        exlosionAudio.Play();
        exlosionEffect.Play();
        Invoke(nameof(ReturnToPool),2f);
    }

    private void ReturnToPool()
    {
        if(gameObject.activeSelf)
         pool.ReturnObject(gameObject);
    }
}
