using UnityEngine;
using Photon.Pun;

public class Cannon : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private ObjectPool bombPool;
    [SerializeField] GameEvent Event_OutOfBoom;

    [Header("Settings")]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] int totalAmountOfBoomb = 25;

    private float lastFireTime;

    void Start()
    {
        UIManager.Instance.UpdateAmmunationInfo(totalAmountOfBoomb);
        bombPool = GameAssets.Instance.cannonPool;
    }


    public void Fire()
    {
        if (!photonView.IsMine)
            return;

        if(totalAmountOfBoomb <= 0)
        {
            //no ammo;
            Event_OutOfBoom.Raise(this,true);
            UIManager.Instance.NoAmmoSignel();
            return;
        }

        if (Time.time < lastFireTime + fireRate)
            return;

        lastFireTime = Time.time;

        // Spawn visual bomb locally
        SpawnLocalBomb();
        

        // Tell Master to spawn logical bomb
        photonView.RPC(
            nameof(SpawnClientBomb),
            RpcTarget.Others,
            firePoint.position,
            firePoint.rotation
        );

        totalAmountOfBoomb --;

        UIManager.Instance.UpdateAmmunationInfo(totalAmountOfBoomb);
        
    }

    private void SpawnLocalBomb()
    {
        GameObject bombObj = bombPool.GetObject();
        bombObj.transform.position = firePoint.position;
        bombObj.transform.rotation = firePoint.rotation;

        BombProjectile bomb = bombObj.GetComponent<BombProjectile>();
        bomb.Initialize(bombPool, true); // false = visual only
    }

    [PunRPC]
    private void SpawnClientBomb(Vector3 position, Quaternion rotation)
    {


        GameObject bombObj = bombPool.GetObject();
        bombObj.transform.position = position;
        bombObj.transform.rotation = rotation;

        BombProjectile bomb = bombObj.GetComponent<BombProjectile>();
        bomb.Initialize(bombPool, false); // true = authoritative
        totalAmountOfBoomb --;

        UIManager.Instance.UpdateAmmunationInfo(totalAmountOfBoomb);
    }
}

