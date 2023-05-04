using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Multiplayer_NetworkManager : MonoBehaviourPunCallbacks
{
    public const float quitDelay = 0.5f;
    public static Multiplayer_NetworkManager Instance;

    [SerializeField] TMP_Text errorText;                // Error message text field variable to be displayed
    [SerializeField] TMP_Text roomNameText;             // Room Name label header
    [SerializeField] Transform playerListContent;       // Transform component for all the player item prefabs
    [SerializeField] GameObject playerListItemPrefab;   // Object for each players name that has joined a room
    [SerializeField] GameObject startGameButton;        // Object for master player to start game

    /// <summary>
    /// Creates a unique instance of Multiplayer_NetworkManager
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Start method which is called when this instance is created.
    /// </summary>
    void Start()
    {
        Debug.Log("Connecting to Dion Server");
        //Connect to the Photon Server
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Override method for player disconnecting to set multiplayer status
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon Server");
        base.OnDisconnected(cause);
        Game_RuntimeData.isMultiplayer = false;  //Sets the multiplayer flag to false
        Debug.Log("isMultiplayer has been set to: "+Game_RuntimeData.isMultiplayer);

    }

    /// <summary>
    /// Override method upon player connecting to server.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        PhotonNetwork.JoinLobby();  // Join the main lobby of multiplayer
        PhotonNetwork.AutomaticallySyncScene = true;    //Syncs all slave clients to start scene when the Master changes
        Game_RuntimeData.isMultiplayer = true;      //Sets the multiplayer state to true
        Debug.Log("isMultiplayer has been set to: " + Game_RuntimeData.isMultiplayer);

        Game_GameState.GetGameMapScenes();
    }

     /// <summary>
     /// Override method upon player successfully joining the lobby set photon nickname and open multiplayer menu.
     /// </summary>
    public override void OnJoinedLobby()
    {
        Multiplayer_MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");

        // Randomly set a player name for each person joining Multiplayer
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        Debug.Log(PhotonNetwork.NickName + " Joined Lobby");
    }

    /// <summary>
    /// Find Room method to join the existing game or create new if non existent.
    /// </summary>
    public void FindRoom()
    {
        RoomOptions options = new RoomOptions();    //Creates a RoomOptions object to be set for the room
        options.MaxPlayers = 16;        //Limit the room to X players
        PhotonNetwork.JoinOrCreateRoom("default_room",options, TypedLobby.Default);     //Creates or joins the room
    }

    /// <summary>
    /// Override method upon player succesfully joining the room. Updates all clients for player names and changes.
    /// </summary>
    public override void OnJoinedRoom()
    {
        Multiplayer_MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Room "+roomNameText.text + " was joined by "+PhotonNetwork.NickName+".");

        Player[] players = PhotonNetwork.PlayerList;    //Sets the player list

        //Destroys all player name objects in the room
        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        //Instantiate a player name for each player that is in the room
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<Multiplayer_PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //Sets the start game visible for Master Client
    }

    /// <summary>
    /// Override method for master client visible objects.
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);    //Permits master player to see and start game
    }
    /// <summary>
    /// Override method if room creation fails
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        Multiplayer_MenuManager.Instance.OpenMenu("error");
        Debug.Log("Error: Room Failed to Create.");
    }

    /// <summary>
    /// Method to quit multiplayer and return to Main Multiplayer_MenuItem.
    /// </summary>
    public void QuitMultiplayer()
    {
        Debug.Log("Quit Multiplayer Invoked - Returning to Main Multiplayer_MenuItem.");
        PhotonView PV = GetComponent<PhotonView>();
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(999));
        PhotonNetwork.Disconnect();
        Game_RuntimeData.isMultiplayer = false;      //Sets the multiplayer state to false

        StartCoroutine(QuitAfterDelay());
    }

     /// <summary>
     /// Method to delay quitting and wait to disconnect from Photon Server.
     /// </summary>
    IEnumerator QuitAfterDelay()
    {
        while (true)
        {
            if (!PhotonNetwork.IsConnected) {
                break;
            }
            yield return null;
        }
        Game_GameState.NextScene("Lobby");
    }

    /// <summary>
    /// Method to permit player to leave the room.
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Multiplayer_MenuManager.Instance.OpenMenu("loading");
    }

    /// <summary>
    /// Override method upon player successfully leaving room, opens multiplayer menu
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.Log("Player left the room.");
        Multiplayer_MenuManager.Instance.OpenMenu("title");
    }

    /// <summary>
    /// Override Method to create instance of a player object joining the room
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<Multiplayer_PlayerListItem>().SetUp(newPlayer);
    }

    /// <summary>
    /// Method to start the game and change scenes
    /// </summary>
    public void StartGame()
    {
        Debug.Log(PhotonNetwork.NickName+" has started a Game!");
        PhotonNetwork.LoadLevel(Data_Scenes.Multiplayer_GameMap_Default);
    }
}
