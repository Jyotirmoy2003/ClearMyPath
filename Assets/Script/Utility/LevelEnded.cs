using Photon.Pun;
using TMPro;
using UnityEngine;

public class LevelEnded : MonoBehaviour
{
    [TextArea(4,10)]
    [SerializeField] string messg;

    [SerializeField] TMP_Text timeText;
    [SerializeField] UIPopInoutBase timerContainer;
    [SerializeField] LeaderboardManager leaderboardManager;

    private bool publishScore = false;

    void Start()
    {
        FirebaseSessionLogger.Instance.AddLog("They reached end");
        Invoke(nameof(ShowLog),2f);
        SetCursorState(false);

        
    }

    void ShowLog()
    {
        Typewriter.Instance.AC_OnTypingFinished += OnTypingDone;
        Typewriter.Instance.StartTyping(messg);
        SetCursorState(false);
    }

    public void Quit()
    {
        WebglJavascriptBridge.Instance.OnApicationQuit();
        Application.Quit();
    }

    private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}


    void OnTypingDone()
    {
        Typewriter.Instance.AC_OnTypingFinished -= OnTypingDone;
        timeText.text =TimeFormatUtility.FormatTime(TimeCounter.Instance.GetFinalTime());
        if(PhotonNetwork.IsMasterClient)
            timerContainer.TogglePop(true);
    }

    #region Publihs Score
    public void onPressPublishScroeYes()
    {
        publishScore = true;
        UpdateDatabase();
        //reaload leaderboard data
        leaderboardManager.RefetchLeaderboard();
    }

    public void OnPressPublihsScroeNo()
    {
        publishScore = false;
        UpdateDatabase();
    }

    void UpdateDatabase()
    {
        timerContainer.TogglePop(false);
        float finalTime = TimeCounter.Instance.GetFinalTime();

        FirebaseSessionLogger.Instance.SetCompletionTime(finalTime, publishScore);

        FirebaseSessionLogger.Instance.EndAndUploadSession();
        
    }

    #endregion
}
