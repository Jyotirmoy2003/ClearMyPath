using UnityEngine;
using TMPro;

public class TimeCounter : MonoSingleton<TimeCounter>
{

    [SerializeField] private TMP_Text timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        UpdateUI();
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetFinalTime()
    {
        return elapsedTime;
    }

    private void UpdateUI()
    {
        if(!timerText) return;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
