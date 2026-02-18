using TMPro;
using UnityEngine;

public class LeaderBoardLable : MonoBehaviour
{
    [SerializeField] TMP_Text rankText;
    [SerializeField] TMP_Text player1;
    [SerializeField] TMP_Text player2;
    [SerializeField] TMP_Text timeTookToComplete;

    public void SetData(int rank,string player1Name,string player2Name,float completionTime)
    {
        rankText.text = rank.ToString();
        player1.text = player1Name;
        player2.text = player2Name;

        int minutes = Mathf.FloorToInt(completionTime / 60f);
        int seconds = Mathf.FloorToInt(completionTime % 60f);
        timeTookToComplete.text = $"{minutes:00}:{seconds:00}";
    }
}
