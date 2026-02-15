using UnityEngine;
using TMPro;
using Photon.Pun;

public class PingFPSDebugger : MonoSingleton<PingFPSDebugger>
{
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private float updateInterval = 0.5f;

    private float deltaTime = 0.0f;
    private float timer;
    private int ping;

    private void Update()
    {
        // --- FPS Calculation ---
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;

        // --- Update Text Every Interval ---
        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;

            ping = PhotonNetwork.IsConnected ? PhotonNetwork.GetPing() : 0;

            debugText.text = $"FPS: {Mathf.CeilToInt(fps)}\nPing: {ping} ms";
        }
    }

    public int GetCurrentPing()
    {
        return ping;
    }
}
