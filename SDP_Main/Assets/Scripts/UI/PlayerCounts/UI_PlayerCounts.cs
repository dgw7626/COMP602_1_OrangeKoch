/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Hanul Rheem		            *
 * Student ID: 		20109218		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This UI_PlayerCounts displays current user in the team UI and (alive/active).
/// </summary>
public class UI_PlayerCounts : MonoBehaviour
{
    public TextMeshProUGUI totalLeftPlayers; //Object for the team text component.
    public TextMeshProUGUI totalRightPlayers;//Object for the other team text component
    public TextMeshProUGUI currentPlayer;    //objext for the current player user name text component.
    public Material purple; // default color for team 2
    public Material orange; // default color for team 1
    public Transform viewLeftPlayers; 
    public Transform viewRightPlayers;
    internal Transform leftView;
    internal Transform rightView;
    internal int numTeams = 2;
    internal int teamOne = 0;
    internal int teamTwo = 0;
    /// <summary>
    /// Start method which is called when this instance is created.
    /// </summary>
    void Start()
    {
        //Initialize the local variable instances.
        currentPlayer = transform.Find("CurrentPlayer").GetComponent<TextMeshProUGUI>();
        totalLeftPlayers = transform.Find("LeftPlayers").Find("TotalLeftPlayers").GetComponent<TextMeshProUGUI>();
        totalRightPlayers = transform.Find("RightPlayers").Find("TotalRightPlayers").GetComponent<TextMeshProUGUI>();
        viewLeftPlayers = totalLeftPlayers.transform.parent;
        viewRightPlayers = totalRightPlayers.transform.parent;
        leftView = viewLeftPlayers.Find("ViewPlayers");
        rightView = viewRightPlayers.Find("ViewPlayers");
        //if its multiplayer
        if (Game_RuntimeData.isMultiplayer)
        {
            //start initializing team members and add empty lists of player_multiplayer entities.
            for (int i = 0; i < numTeams; i++)
            {
                Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
            }
            //get all the instantiated paleyrs and start adding them too.
            for (int j = 0; j < Game_RuntimeData.instantiatedPlayers.Count; j++)
            {

                // Purple team
                if (Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber % 2 == 0)
                {
                    teamTwo++;
                    //instnatiate 2D ui element so the player can view it.
                    var viewLayers = Instantiate(
                    Resources.Load(Path.Combine("LocalPrefabs", "PlayerStatusUI")) as GameObject,
                    Vector3.zero,
                    Quaternion.identity,
                    leftView);
                    viewLayers.GetComponent<Image>().color = purple.color;
                    //set text to current player user id.
                    viewLayers.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber.ToString();
                }
                // Orange team
                else
                {
                    teamOne++;
                    //instantiate 2D ui element so the player can view it.
                    var viewLayers = Instantiate(
                   Resources.Load(Path.Combine("LocalPrefabs", "PlayerStatusUI")) as GameObject,
                   Vector3.zero,
                   Quaternion.identity,
                   rightView);
                    viewLayers.GetComponent<Image>().color = orange.color;
                    //set text to current player user id.
                    viewLayers.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber.ToString();
                }
            }
            // added player identifier.
            currentPlayer.text = "You are: Player " + Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber;
            //check if the actor number is odd or whole number.
            if (Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber % 2 == 0)
            {
                currentPlayer.color = purple.color;
            }
            else
            {
                currentPlayer.color = orange.color;
            }
            //print out with current team and number of total players each team.
            totalLeftPlayers.text = "Team " + (numTeams - 1) + "\n Total Players: " + teamTwo;
            totalRightPlayers.text = "Team " + (numTeams) + "\n Total Players: " + teamOne;

        }
    }
    /// <summary>
    /// This method removes player instance when the player leave the game, it returns nothing.
    /// </summary>
    public void RemovePlayerCounts()
    {
        //get all the players instances
        TextMeshProUGUI[] totalLeft = leftView.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] totalRight = rightView.GetComponentsInChildren<TextMeshProUGUI>();
        int playerNumber = Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber;

        //if the player is multiplayer
        if (Game_RuntimeData.isMultiplayer)
        {
            //check if the player contains with the current user id.
            foreach (TextMeshProUGUI player in totalLeft)
            {
                if (player != null)
                {
                    if (player.text.Contains(playerNumber.ToString()))
                    {
                        // if the player left destroy it and decrease by 1.
                        Destroy(player.gameObject);
                        teamOne--;
                    }
                }
            }
            //check if the player contains with the current user id.
            foreach (TextMeshProUGUI player in totalRight)
            {
                Debug.Log(player.transform.name);
                if (player != null)
                {
                    if (player.text.Contains(playerNumber.ToString()))
                    {  
                        // if the player left destroy it and decrease by 1.
                        Destroy(player.gameObject);
                        teamTwo--;
                    }
                }
            }
            //print out with current team and number of total players each team.
            totalLeftPlayers.text = "Team " + (numTeams - 1) + "\n Total Players: " + teamTwo;
            totalRightPlayers.text = "Team " + (numTeams) + "\n Total Players: " + teamOne;
        }
    }
}
