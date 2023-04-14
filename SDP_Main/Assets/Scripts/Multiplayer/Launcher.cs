using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public const float quitDelay = 0.5f;
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Dion Server");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Dion Server");
        base.OnDisconnected(cause);
        Game_RuntimeData.isMultiplayer = false;
        Debug.Log("isMultiplayer has been set to: "+Game_RuntimeData.isMultiplayer);

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Dion Server");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        Game_RuntimeData.isMultiplayer = true;
        Debug.Log("isMultiplayer has been set to: " + Game_RuntimeData.isMultiplayer);
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void FindRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 16;
        PhotonNetwork.JoinOrCreateRoom("default_room",options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Room "+roomNameText.text + " was joined by "+PhotonNetwork.NickName+"."); 

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
        Debug.Log("Error: Room Failed to Create.");
    }
    public void QuitMultiplayer()
    {
        Debug.Log("Quit Multiplayer Invoked - Returning to Main Menu.");
        PhotonView PV = GetComponent<PhotonView>();
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(999));
        PhotonNetwork.Disconnect();

        StartCoroutine(QuitAfterDelay());
    }

    IEnumerator QuitAfterDelay()
    {
        while (true)
        {
            int i = 0;
            if (!PhotonNetwork.IsConnected) { 
                break;    
            }
            yield return null;
            Debug.Log(i++);

        }
       // yield return new WaitForSeconds(quitDelay);
        
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Player left the room " + roomNameInputField.text + ".");
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {
        Debug.Log(PhotonNetwork.NickName+" has started a Game!");
        PhotonNetwork.LoadLevel("Game");
    }
}
