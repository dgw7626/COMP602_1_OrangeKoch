using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCounts : MonoBehaviour
{
    public TextMeshProUGUI totalLeftPlayers;
    public TextMeshProUGUI totalRightPlayers;
    public TextMeshProUGUI currentPlayer;
    public Material purple;
    public Material orange;
    public Transform viewLeftPlayers;
    public Transform viewRightPlayers;
    internal Transform leftView;
    internal Transform rightView;
    private int numPlayers = 0;
    internal int numTeams = 2;
    internal int teamOne = 0;
    internal int teamTwo = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Get Plaeyr 
        currentPlayer = transform.Find("CurrentPlayer").GetComponent<TextMeshProUGUI>();
        totalLeftPlayers = transform.Find("LeftPlayers").Find("TotalLeftPlayers").GetComponent<TextMeshProUGUI>();
        totalRightPlayers = transform.Find("RightPlayers").Find("TotalRightPlayers").GetComponent<TextMeshProUGUI>();
        viewLeftPlayers = totalLeftPlayers.transform.parent;
        viewRightPlayers = totalRightPlayers.transform.parent;
        leftView = viewLeftPlayers.Find("ViewPlayers");
        rightView = viewRightPlayers.Find("ViewPlayers");
        if (Game_RuntimeData.isMultiplayer)
        {
            for (int i = 0; i < numTeams; i++)
            {
                Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
            }
     
            for (int j = 0; j < Game_RuntimeData.instantiatedPlayers.Count; j++)
        {
         
            if (Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber % 2 == 0)
            {
                    Debug.Log("test");
                teamTwo++;
                    var viewLayers = Instantiate(
                    Resources.Load(Path.Combine("LocalPrefabs", "PlayerStatusUI")) as GameObject,
                    Vector3.zero,
                    Quaternion.identity,
                    leftView);
                    viewLayers.GetComponent<Image>().color = purple.color;

                    viewLayers.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber.ToString();
                }
            else 
            {
                teamOne++;
                    var viewLayers = Instantiate(
                   Resources.Load(Path.Combine("LocalPrefabs", "PlayerStatusUI")) as GameObject,
                   Vector3.zero,
                   Quaternion.identity,
                   rightView);
                    viewLayers.GetComponent<Image>().color = orange.color;
                    viewLayers.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Game_RuntimeData.instantiatedPlayers[j].photonView.Owner.ActorNumber.ToString();
                }
                numPlayers++;
            }
            currentPlayer.text = "You are: Player " + Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber;
            if (Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber % 2 == 0) {
                currentPlayer.color = purple.color;
            }
            else
            {
                currentPlayer.color = orange.color;
            }
            totalLeftPlayers.text = "Team " + (numTeams -1) + "\n Total Players: " + teamTwo;
            totalRightPlayers.text = "Team " + (numTeams) + "\n Total Players: " + teamOne;
     
        }
    }
}
