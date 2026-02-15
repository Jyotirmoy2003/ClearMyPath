using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmojiButton : MonoBehaviour
{
   [SerializeField] string particleName;
   [SerializeField] Button button;
   [SerializeField] PhotonView pv;

    [SerializeField] float cooldownTime = 3f;
    private bool isReadyToPlay = true;
    

    public void OnButtonPressed()
    {
        if(!isReadyToPlay) return;
        
        isReadyToPlay = false;
        pv.RPC(nameof(PlayParticleOnMultiplayer),RpcTarget.All);
        
        
    }

    [PunRPC]
    void PlayParticleOnMultiplayer()
    {
        Debug.Log("Recived rpc for emoji");
        ParticleManager.Instance.PlayParticle(particleName);
        Invoke(nameof(CoolDownDone),cooldownTime);
    }

    void CoolDownDone()
    {
        isReadyToPlay = true;
    }
}
