using System.Collections;
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

    [SerializeField] TMP_Text errorText;                // Error message text field variable to be displayed
    [SerializeField] TMP_Text roomNameText;             // Room Name label header
    [SerializeField] Transform playerListContent;       // Transform component for all the player item prefabs
    [SerializeField] GameObject playerListItemPrefab;   // Object for each players name that has joined a room
    [SerializeField] GameObject startGameButton;        // Object for master player to start game

    /*
     * Creates a unique instance of Launcher
     */
    void Awake()
    {
        Instance = this;
    }

    /*
     * Start method which is called when this instance is created.
     */
    void Start()
    {
        Debug.Log("Connecting to Dion Server");
        //Connect to the Photon Server
        PhotonNetwork.ConnectUsingSettings();
    }

    /*
     * Override method for player disconnecting to set multiplayer status
     */
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Dion Server");
        base.OnDisconnected(cause);
        Game_RuntimeData.isMultiplayer = false;  //Sets the multiplayer flag to false
        Debug.Log("isMultiplayer has been set to: "+Game_RuntimeData.isMultiplayer);

    }

    /*
     * Override method upon player connecting to server.
     */
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Dion Server");
        PhotonNetwork.JoinLobby();  // Join the main lobby of multiplayer
        PhotonNetwork.AutomaticallySyncScene = true;    //Syncs all slave clients to start scene when the Master changes
        Game_RuntimeData.isMultiplayer = true;      //Sets the multiplayer flag to true
        Debug.Log("isMultiplayer has been set to: " + Game_RuntimeData.isMultiplayer);
    }

    /*
     * Override method upon player successfully joining the lobby set photon nickname and open multiplayer menu.
     */
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");

        // Randomly set a player name for each person joining Multiplayer
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    /*
     * Find Room method to join the existing game or create new if non existent.
     */
    public void FindRoom()
    {
        RoomOptions options = new RoomOptions();    //Creates a RoomOptions object to be set for the room
        options.MaxPlayers = 16;        //Limit the room to X players
        PhotonNetwork.JoinOrCreateRoom("default_room",options, TypedLobby.Default);     //Creates or joins the room
    }

    /*
     * Override method upon player succesfully joining the room. Updates all clients for player names and changes.
     */
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
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
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient); //Sets the start game visible for Master Client
    }

    /*
     *  Override method for master client visible objects.
     */
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);    //Permits master player to see and start game
    }

    /*
     * Override method if room creation fails
     */
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
        Debug.Log("Error: Room Failed to Create.");
    }

    /*
     * Method to quit multiplayer and return to Main Menu.
     */
    public void QuitMultiplayer()
    {
        Debug.Log("Quit Multiplayer Invoked - Returning to Main Menu.");
        PhotonView PV = GetComponent<PhotonView>();
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(999));
        PhotonNetwork.Disconnect();

        StartCoroutine(QuitAfterDelay());
    }

    /*
     * Method to delay quitting and wait to disconnect from Photon Server.
     */
    IEnumerator QuitAfterDelay()
    {
        while (true)
        {
            if (!PhotonNetwork.IsConnected) { 
                break;    
            }
            yield return null;
        }
        
        SceneManager.LoadScene("Lobby");
    }

    /*
     * Method to permit player to leave the room.
     */
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    /*
     * Override method upon player successfully leaving room, opens multiplayer menu
     */
    public override void OnLeftRoom()
    {
        Debug.Log("Player left the room.");
        MenuManager.Instance.OpenMenu("title");
    }

    /*
     * Override Method to create instance of a player object joining the room
     */
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    /*
     * Method to start the game and change scenes
     */
    public void StartGame()
    {
        Debug.Log(PhotonNetwork.NickName+" has started a Game!");
        PhotonNetwork.LoadLevel("Game");
    }
}
