using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using NaughtyAttributes;

public class RoomReadyManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Button readyStartButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text countdownText;
    [Header("Visual Icon Ref")]
    [Foldout("Avater")][SerializeField] Image maleAvater;
    [Foldout("Avater")][SerializeField] GameObject fadedMaleAvater;
    [Foldout("Avater")][SerializeField] List<Sprite> avialbeMaleAvaters = new List<Sprite>();
    [Foldout("Avater")][SerializeField] Image femaleAvater;
    [Foldout("Avater")][SerializeField] GameObject fadedFemaleAvater;
    [Foldout("Avater")][SerializeField] List<Sprite> avialbeFemaleAvaters = new List<Sprite>();
    [SerializeField] Image readyStatusMaster;
    [SerializeField] Image readyStatusClient;
    [SerializeField] Sprite checkMark;
    [SerializeField] Sprite cross;
    

    private const string READY_KEY = "Ready";
    private bool gameStarting = false;

    private int selectedMaleAvaterIndex = 0;
    private int selectedFemaleAvaterIndex = 0;
    private const string MALE_AVATAR_KEY = "MaleAvatar";
    private const string FEMALE_AVATAR_KEY = "FemaleAvatar";

    

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SetupButton();
        UpdateButtonState();
        countdownText.text = "";

        //when joined room master data always stays same
        readyStatusMaster.sprite = checkMark;

        SelectAvaterMale();
        FetchExistingAvatars();
       

        CheckPlayerAndShowMessg();
    }

    void SelectAvaterMale()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            selectedMaleAvaterIndex = Random.Range(0, avialbeMaleAvaters.Count);

            Hashtable props = new Hashtable
            {
                { MALE_AVATAR_KEY, selectedMaleAvaterIndex }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else
        {
            selectedFemaleAvaterIndex = Random.Range(0, avialbeFemaleAvaters.Count);

            Hashtable props = new Hashtable
            {
                { FEMALE_AVATAR_KEY, selectedFemaleAvaterIndex }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
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
            FirebaseSessionLogger.Instance.StartSession();
            FirebaseSessionLogger.Instance.SetPlayers(PhotonNetwork.PlayerList[0].NickName,"No One Joined yet");
            FirebaseSessionLogger.Instance.AddLog("Master ping: "+PingFPSDebugger.Instance.GetCurrentPing().ToString());
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

            
            
            FirebaseSessionLogger.Instance.SetPlayers(player1,player2);
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
        //avatar
        if (changedProps.ContainsKey(MALE_AVATAR_KEY))
        {
            int index = (int)changedProps[MALE_AVATAR_KEY];
            maleAvater.sprite = avialbeMaleAvaters[index];
            fadedMaleAvater.SetActive(false);
            maleAvater.gameObject.SetActive(true);
        }

        if (changedProps.ContainsKey(FEMALE_AVATAR_KEY))
        {
            int index = (int)changedProps[FEMALE_AVATAR_KEY];
            femaleAvater.sprite = avialbeFemaleAvaters[index];
            fadedFemaleAvater.SetActive(false);
            femaleAvater.gameObject.SetActive(true);
        }
        //ready
        if (changedProps.ContainsKey(READY_KEY))
        {
            readyStatusClient.sprite = checkMark;
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
        
        #if UNITY_EDITOR
        readyStartButton.interactable = true;
        #endif
    }

    void FetchExistingAvatars()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey(MALE_AVATAR_KEY))
            {
                int index = (int)player.CustomProperties[MALE_AVATAR_KEY];
                maleAvater.sprite = avialbeMaleAvaters[index];
            }

            if (player.CustomProperties.ContainsKey(FEMALE_AVATAR_KEY))
            {
                int index = (int)player.CustomProperties[FEMALE_AVATAR_KEY];
                femaleAvater.sprite = avialbeFemaleAvaters[index];
            }
        }
    }


    #endregion
}
