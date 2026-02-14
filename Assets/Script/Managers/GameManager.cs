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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackPressed();
        }
    }

    private void OnBackPressed()
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
        OnBackPressed();
    }


    public void OnQuiteButtonPressed()
    {
        Application.Quit();
    }

    private void OnApplicationFocus(bool hasFocus)
	{
        SetCursorState(!isGamePaused);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
