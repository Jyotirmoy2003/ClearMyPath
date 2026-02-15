using UnityEngine;
using System.Runtime.InteropServices;

public class WebglJavascriptBridge : MonoSingleton<WebglJavascriptBridge>
{
    [DllImport("__Internal")]
    private static extern void ExitFullscreen();
    [DllImport("__Internal")]
    private static extern void ReloadPage();


    public void OnApicationQuit()
    {
        // Leave Photon room safely
        if (Photon.Pun.PhotonNetwork.InRoom)
            Photon.Pun.PhotonNetwork.LeaveRoom();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            ExitFullscreen();
            ReloadPage();
        }
    }

    #region Copy to Clipboard
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    public void CopyRoomCode(string roomCode)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            CopyToClipboard(roomCode);
        }
        else
        {
            GUIUtility.systemCopyBuffer = roomCode;
        }
    }

    #endregion


        #region WEBGL Firebase

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadSession(string json);
#endif

    public void UploadToJS(string json)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UploadSession(json);

#endif
    }

    public void OnBrowserClosing()
    {
        FirebaseSessionLogger.Instance.EndAndUploadSession();
    }


    #endregion
}
