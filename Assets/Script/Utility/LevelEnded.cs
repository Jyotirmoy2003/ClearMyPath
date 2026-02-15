using UnityEngine;

public class LevelEnded : MonoBehaviour
{
    [TextArea(4,10)]
    [SerializeField] string messg;
    void Start()
    {
        FirebaseSessionLogger.Instance.AddLog("They reached end");
        Invoke(nameof(ShowLog),2f);
        SetCursorState(false);
    }

    void ShowLog()
    {
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
}
