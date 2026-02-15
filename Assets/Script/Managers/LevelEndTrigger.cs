using UnityEngine;
using Photon.Pun;

public class LevelEndTrigger : MonoBehaviour
{
    private bool levelEnding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelEnding)
            return;

        // Only Master decides
        if (!PhotonNetwork.IsMasterClient)
            return;

        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            levelEnding = true;

            Debug.Log("Level Complete");

            LevelLoader.Instance.LoadNextLevel();
            AudioManager.instance.PlaySound("Win");
        }
    }
}
