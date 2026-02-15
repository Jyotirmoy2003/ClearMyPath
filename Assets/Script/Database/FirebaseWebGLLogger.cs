using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class FirebaseWebGLLogger : MonoBehaviour
{
    public static FirebaseWebGLLogger Instance;

    [SerializeField] private string databaseURL = 
        "https://yourproject-default-rtdb.firebaseio.com/";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UploadSession(string jsonData)
    {
        StartCoroutine(PostData(jsonData));
    }

    private IEnumerator PostData(string json)
    {
        string url = databaseURL + "GameSessions.json";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Firebase upload success!");
        }
        else
        {
            Debug.LogError("Firebase upload failed: " + request.error);
        }
    }
}
