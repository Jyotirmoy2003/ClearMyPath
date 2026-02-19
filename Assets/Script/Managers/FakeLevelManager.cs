using Photon.Pun;
using UnityEngine;

public class FakeLevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            if(LevelLoader.Instance.lastPlayedLevelIndex >-1)
                LevelLoader.Instance.LoadLevel(LevelLoader.Instance.lastPlayedLevelIndex);
    }

   
}
