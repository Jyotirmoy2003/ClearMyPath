using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>() != null)
        {
            CheckpointSystem.Instance.SetCheckpoint(spawnPoint);
        }
    }
}
