using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool isGamePaused = false;

    void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            SetCursorState(false);
        }

        if(Input.GetMouseButtonDown(2))
        {
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
