using System.Runtime.InteropServices;
using UnityEngine;

public class ClipboardHelper : MonoSingleton<ClipboardHelper>
{
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
}
