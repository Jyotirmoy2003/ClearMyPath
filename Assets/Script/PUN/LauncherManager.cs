using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using NaughtyAttributes;

public class LauncherManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInput;
    

    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject connectPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] TMP_Text connectPanelText;
    [Foldout("Room")][SerializeField] TMP_Text roomNameText;
    [Foldout("Room")][SerializeField] TMP_Text masterPlayerName;
    [Foldout("Room")][SerializeField] TMP_Text clientPlayerName;
    [Foldout("Room")][SerializeField] GameObject joinRoomPanel;
    [Foldout("Room")][SerializeField] private TMP_InputField joinCodeInput;
    

    private const byte maxPlayers = 2;

    void Start()
    {
        DisableAllPanel();
        namePanel.SetActive(true);   
    }

    void DisableAllPanel()
    {
        namePanel.SetActive(false);
        connectPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
        joinRoomPanel.SetActive(false);
    }

    #region CONNECT

    void ConnectToPhoton()
    {
        if (string.IsNullOrEmpty(nameInput.text))
            return;

        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        connectPanel.SetActive(true);
        connectPanelText.text = "Connecting to server..";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        DisableAllPanel();
        lobbyPanel.SetActive(true);
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause);

        connectPanel.SetActive(true);
        connectPanelText.text = "Please refresh the page, cause:\n"+cause;
    }

    #endregion

    #region CREATE ROOM

    public void CreateRoomButtonPressed()
    {
        string roomCode = GenerateRoomCode();

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayers
        };

        PhotonNetwork.CreateRoom(roomCode, options);
        DisableAllPanel();
        connectPanel.SetActive(true);
        connectPanelText.text = "Creating room..";
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
        roomNameText.text = "Room Name: "+PhotonNetwork.CurrentRoom.Name;


    }

    public void OnCopyRoomCodeButtonPressed()
    {
        if (PhotonNetwork.InRoom)
        {
            string roomCode = PhotonNetwork.CurrentRoom.Name;
            ClipboardHelper.Instance.CopyRoomCode(roomCode);
            //UIManager.Instance.PunchUI(copyButtonRect, new Vector2(20, 0), 0.2f);

        }
    }

    public override void OnJoinedRoom()
    {
        DisableAllPanel();
        roomPanel.SetActive(true);
        roomNameText.text = "Room Name: "+PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerNames();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerNames();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerNames();
    }

    private void UpdatePlayerNames()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                masterPlayerName.text = player.NickName;
            }
            else
            {
                clientPlayerName.text = player.NickName;
            }
        }
    }

    #endregion

    #region JOIN ROOM


    #region  Leave Room
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        DisableAllPanel();
        lobbyPanel.SetActive(true);
    }


    #endregion
    public void OnJoinRoomPanelButton()
    {
        DisableAllPanel();
        joinRoomPanel.SetActive(true);
    }
    public void OnJoinRoomPressed()
    {
        if (string.IsNullOrEmpty(joinCodeInput.text))
            return;

        PhotonNetwork.JoinRoom(joinCodeInput.text.ToUpper());
    }

   
    #endregion

    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string code = "";

        for (int i = 0; i < 6; i++)
        {
            code += chars[UnityEngine.Random.Range(0, chars.Length)];
        }

        return code;
    }



    public void OnNameSubmitButtonPressed()
    {
       ConnectToPhoton();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
