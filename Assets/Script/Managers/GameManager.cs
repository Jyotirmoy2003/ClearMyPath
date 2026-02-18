using System.Collections;
using System.Collections.Generic;
using EasyPopupSystem;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool isGamePaused = false;
    [SerializeField] float gamevalidateTime = 3f;
    private SpawnHelpr[] validateNumberOfSpawnHelper;

    void Start()
    {
        Invoke(nameof(ValidateGame),gamevalidateTime);
    }

    void ValidateGame()
    {
        validateNumberOfSpawnHelper = FindObjectsOfType<SpawnHelpr>();

        if(validateNumberOfSpawnHelper.Length != 2)
        {
            //something went wrong
            EasyPopupManager.Instance.CreateToast("ErrorNotValidGame");
            
            if(validateNumberOfSpawnHelper.Length > 2)
            {
                int lastBiggestNumber = 0;
                SpawnHelpr havetoKill = null;
                foreach(SpawnHelpr item in validateNumberOfSpawnHelper)
                {
                    if(item.TryGetComponent<PhotonView>(out var photonView))
                    {
                        if(photonView.ViewID > lastBiggestNumber)
                        {
                            havetoKill = item;
                            lastBiggestNumber = photonView.ViewID;
                        }
                    }
                }

                if(havetoKill != null)
                {
                    //Destroy(havetoKill.gameObject);
                }
            }
            else
            {
                Invoke(nameof(RequestRestartGame),3f);
            }
        }
    }

    void RequestRestartGame()
    {
    #if !UNITY_EDITOR
       WebglJavascriptBridge.Instance.OnApicationQuit();
    #endif
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            SetCursorState(false);
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(isGamePaused)
            {
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            ToggleCursorState();
        }
    }

    public void OnSettingsPressed()
    {
        Debug.Log("Back button pressed");

        isGamePaused = !isGamePaused;
        UIManager.Instance.SetPauseMenuStatus(isGamePaused);

        SetCursorState(!isGamePaused);
        if(GameAssets.Instance.starterAssetsInputs)
            GameAssets.Instance.starterAssetsInputs.enabled = !isGamePaused;

    }

    public void OnRestartButtonPressed()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        UIManager.Instance.BlackScreenFadeIn();

        yield return new WaitForSeconds(1f);

        LevelLoader.Instance.RestartLevel();
    }
    public void Resumelevel()
    {
        OnSettingsPressed();
    }


    public void OnQuiteButtonPressed()
    {
        WebglJavascriptBridge.Instance.OnApicationQuit();
        Application.Quit();
    }

    

	public void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}

    public void ToggleCursorState()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
