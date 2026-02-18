using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CheckpointSystem : MonoSingleton<CheckpointSystem>
{

    [SerializeField] private float respawnDelay = 4f;
    [SerializeField] GameEvent Event_OnPlayerRespawned;
     [Header("Events")]
    public UnityEvent playerRespawned;

    private Transform currentCheckpoint;
    private bool isRespawning = false;


    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void RespawnPlayer(Transform player)
    {
        if(isRespawning) return;
        isRespawning = true;
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(Transform player)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (currentCheckpoint != null)
        {
            player.position = currentCheckpoint.position;
            player.rotation = currentCheckpoint.rotation;
            //notify
            playerRespawned?.Invoke();
            Event_OnPlayerRespawned.Raise(this,true);
            isRespawning = false;
        }
    }
}
