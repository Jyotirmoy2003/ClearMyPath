using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameTipsManager : MonoBehaviour
{
    [TextArea(3,10)]
    [SerializeField] string messgForBoy;
    [TextArea(3,10)]
    [SerializeField] string messgForGirl;

    

    void Start()
    {
        Invoke(nameof(LetUserKnowTheMessg),6f);
    }

    void LetUserKnowTheMessg()
    {
        AudioManager.instance.PlaySound("BG");
        UIManager.Instance.ActivateDevTalk();
        if(PhotonNetwork.IsMasterClient)
        {
            Typewriter.Instance.StartTyping(messgForBoy);
        }
        else
        {
            Typewriter.Instance.StartTyping(messgForGirl);
        }
    }
}
