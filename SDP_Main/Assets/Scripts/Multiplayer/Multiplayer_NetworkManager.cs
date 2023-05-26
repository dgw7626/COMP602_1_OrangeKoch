/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Dion Hemmes		            *
 * Student ID: 		21154191		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;


public class Multiplayer_NetworkManager : MonoBehaviourPunCallbacks
{
    public const float quitDelay = 0.5f;
    public static Multiplayer_NetworkManager Instance;

    [SerializeField] TMP_Text errorText;                // Error message text field variable to be displayed
    [SerializeField] TMP_Text roomNameText;             // Room Name label header
    [SerializeField] Transform playerListContent;       // Transform component for all the player item prefabs
    [SerializeField] GameObject playerListItemPrefab;   // Object for each players name that has joined a room
    [SerializeField] GameObject startGameButton;        // Object for master player to start game
    [SerializeField] GameObject uiErrorMessage;         // Object for error ui message.

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
        if (!Game_RuntimeData.isMultiplayer) {
            Debug.Log("Connecting to Photon Server");
            //Connect to the Photon Server
            PhotonNetwork.ConnectUsingSettings();
            uiErrorMessage.SetActive(false);
        }
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
          //Creates or joins the room
        PhotonNetwork.JoinOrCreateRoom("default_room",options, TypedLobby.Default);
    }

    /// <summary>
    /// This method override the photon method for when a player fails to join the room.
    /// Once the game is started.This method restrict the new player and show the error messege. 
    /// </summary>
    public override void OnJoinRoomFailed (short returnCode, string message)
    {
        // This photon method activate the error message.
        uiErrorMessage.SetActive(true);

        // This message display the message on the screne.
        uiErrorMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Game Error: game is still running";

        // This message disable the error after 1.2 seconds.
        Invoke(nameof(DisableErrorMessage), 1.2f);
    }

    /// <summary>
    /// This photon method disable error message.
    /// </summary>
    internal void DisableErrorMessage(){
        uiErrorMessage.SetActive(false);
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
    }

    /// <summary>
    /// Method to quit multiplayer and return to Lobby.
    /// </summary>
    public void QuitMultiplayer()
    {
        Debug.Log("Quit Multiplayer Invoked - Returning to Lobby.");
        Game_RuntimeData.CleanUp_Multiplayer_Data();

        // Get a reference to the Photon Voice Manager object
        var voiceManager = GameObject.Find("VoiceManager");
        // If the object exists, stop and destroy the voice service
        if (voiceManager != null)
        {
            Destroy(voiceManager);
        }

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
        PhotonNetwork.LoadLevel(Data_Scenes.Multiplayer_GameMap_Uknown);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
}
