using UnityEngine;
using Photon.Pun;

public class VoidTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

       

        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(1);
        }
    }
}
