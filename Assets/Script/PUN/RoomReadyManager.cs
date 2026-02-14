using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;

public class RoomReadyManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Button readyStartButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text countdownText;

    private const string READY_KEY = "Ready";
    private bool gameStarting = false;

    

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SetupButton();
        UpdateButtonState();
        countdownText.text = "";

        CheckPlayerAndShowMessg();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckPlayerAndShowMessg();
    }

    void CheckPlayerAndShowMessg()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            Typewriter.Instance.StartTyping(
    @"Don’t keep the adventure waiting!

    Send this room code to your favorite human and tell them to join you.
    This story needs two players… and maybe a little love.");
        }
        else
        {
            string player1 = PhotonNetwork.PlayerList[0].NickName;
            string player2 = PhotonNetwork.PlayerList[1].NickName;


            string message =
    $@"Look at you two!

    {player1} and {player2} are finally together ♥

    Press Ready… and let’s see how well this duo survives!";
            
            
            Typewriter.Instance.SkipTyping();
            Typewriter.Instance.StartTyping(message);
        }
    }

    private void SetupButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonText.text = "Start";
        }
        else
        {
            buttonText.text = "Ready";
        }
    }

    public void OnReadyStartButtonPressed()
    {
        if (gameStarting) return;

        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
        else
        {
            SetReady();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log("Player Left: " + otherPlayer.NickName);

        // Stop countdown if running
        if (gameStarting)
        {
            StopAllCoroutines();
            gameStarting = false;
            countdownText.gameObject.SetActive(false);
        }

        // Reset ready state for safety
        ResetReadyState();

        // Disable start button
        readyStartButton.interactable = false;

        // Optionally show waiting message
        buttonText.text = PhotonNetwork.IsMasterClient ? "Start" : "Ready";
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("New Master: " + newMasterClient.NickName);

        SetupButton();
        UpdateButtonState();
    }



    #region READY
    private void ResetReadyState()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { READY_KEY, false }
            };

            player.SetCustomProperties(props);
        }
    }

    private void SetReady()
    {
        Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { READY_KEY, true }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        readyStartButton.interactable = false;
    }

    #endregion

    #region START

    private void StartGame()
    {
        if (!AllPlayersReady()) return;

        gameStarting = true;

        photonView.RPC(nameof(StartCountdown), RpcTarget.All);
    }

    private bool AllPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
                continue;

            if (!player.CustomProperties.ContainsKey(READY_KEY))
                return false;

            if (!(bool)player.CustomProperties[READY_KEY])
                return false;
        }

        return true;
    }

    #endregion

    #region COUNTDOWN

    [PunRPC]
    private void StartCountdown()
    {
        readyStartButton.interactable = false;
        StartCoroutine(CountdownRoutine());
    }

    private System.Collections.IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);

        int count = 5;

        while (count > 0)
        {
            countdownText.text = "Starting in " + count;
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        // Load Game Scene
        PhotonNetwork.LoadLevel("Level1");
    }

    #endregion

    #region SYNC

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(READY_KEY))
        {
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            readyStartButton.interactable = false;
            return;
        }

        readyStartButton.interactable = AllPlayersReady();
    }

    #endregion
}
