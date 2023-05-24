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
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is designed to create a player name object when joining a room within the Multiplayer UI
/// </summary>
public class Multiplayer_PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player player;

    /// <summary>
    /// This method sets the Text Objects to be referred to throughout this class.
    /// </summary>
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// This method updates the players name label, so that it reflects the name it was given.
    /// </summary>
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    /// <summary>
    /// This method destroys another players name object from the list when the player leaves the room.
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method destroys a players name object from the list when the player leaves the room.
    /// </summary>
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
