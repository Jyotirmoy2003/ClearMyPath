using System.Collections.Generic;
using Photon.Pun;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnHelpr : MonoBehaviour
{
    public PhotonView pv;
    [SerializeField] List<GameObject> objectsToDeisable = new List<GameObject>();
    [SerializeField] List<Behaviour> disableComponents = new List<Behaviour>();
    [SerializeField] List<GameObject> disbaleOnMyView = new List<GameObject>();
   [SerializeField] MeshRenderer meshRendererToDisableinMyView;
    [SerializeField] StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
        if(pv == null)
            pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(!pv.IsMine)
        {
            foreach(GameObject item in objectsToDeisable)
            {
                item.SetActive(false);
            }

           foreach(Behaviour item in disableComponents)
            {
                item.enabled = false;
            }
        }
        else
        {
            foreach(GameObject item in disbaleOnMyView)
            {
                item.SetActive(false);
            }
            if(meshRendererToDisableinMyView)
            meshRendererToDisableinMyView.enabled = false;

            UIManager.Instance.SetControllstatus(PhotonNetwork.IsMasterClient);
            if(starterAssetsInputs)
            {
                GameAssets.Instance.starterAssetsInputs = starterAssetsInputs;
            }
            UIManager.Instance.ConfigureControlls(PhotonNetwork.IsMasterClient);

            if(PhotonNetwork.IsMasterClient)
            {
                UIManager.Instance.SetSliderData(8,3);
            }
            else
            {
                UIManager.Instance.SetSliderData(4,0.15f);
            }
        }

        GameManager.Instance.SetCursorState(true);
        Invoke(nameof(ShowScene),2f);
    }

    void ShowScene()
    {

        UIManager.Instance.BlackScreenFadeOut();
    }
}
