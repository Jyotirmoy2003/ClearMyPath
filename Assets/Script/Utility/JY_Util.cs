using System.Collections.Generic;
using UnityEngine;

public class JY_Util : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class SessionLogData
{
    public string sessionId;
    public string player1;
    public string player2;
    public string startTime;
    public string endTime;
    public List<string> logs;

    public SessionLogData(string id, string p1, string p2)
    {
        sessionId = id;
        player1 = p1;
        player2 = p2;
        startTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        logs = new List<string>();
    }
}
