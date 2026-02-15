using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Prefabs (Must be in Resources Folder)")]
    [SerializeField] private string malePrefabName;
    [SerializeField] private string femalePrefabName;

    [Header("Spawn Points")]
    [SerializeField] private Transform maleSpawnPoint;
    [SerializeField] private Transform femaleSpawnPoint;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
            return;

        if (PhotonNetwork.IsMasterClient )
        {
            PhotonNetwork.Instantiate(
                malePrefabName,
                maleSpawnPoint.position,
                maleSpawnPoint.rotation
            );
            GameAssets.Instance.MasterSpawned = true;
        }
        else
        {
            if(!GameAssets.Instance.clientSpawned)
            PhotonNetwork.Instantiate(
                femalePrefabName,
                femaleSpawnPoint.position,
                femaleSpawnPoint.rotation
            );
            GameAssets.Instance.clientSpawned = true;
        }
            CheckpointSystem.Instance.SetCheckpoint(femaleSpawnPoint);
    }
}
