using UnityEngine;
using Photon.Pun;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] ParticleSystem exlosionEffect;
    [SerializeField] AudioSource exlosionAudio;

    private ObjectPool pool;
    private float timer;
    private bool isAuthoritative;

    public void Initialize(ObjectPool objectPool, bool authoritative)
    {
        pool = objectPool;
        isAuthoritative = authoritative;
        timer = lifetime;
        Invoke(nameof(ReturnToPool),lifetime);
    }

    private void Update()
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
