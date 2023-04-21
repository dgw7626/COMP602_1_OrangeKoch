using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MultiplayerEntity : MonoBehaviour//, IPunObservable
{
    public Player_PlayerController playerController;
    public string uniqueID {  get; private set; }

    public bool RegisterUniqueID(string uniqueID)
    {
        if (this.uniqueID != null)
            return false;

        this.uniqueID = uniqueID;
        return true;
    }
    private void Start()
    {
        playerController = GetComponent<Player_PlayerController>();
        if (Game_RuntimeData.isMultiplayer)
        {
            playerController.IsInputLocked = true;
            Game_RuntimeData.instantiatedPlayers.Add(this);
        }
    }

    [PunRPC]
    public void OnDamageRecieved(float damage) 
    {
        if (!playerController.photonView.IsMine)
        {
            Debug.Log("I am Not executing because it is not me");
            return;
        }
        Debug.Log("I am executing because it is me!");
        //TODO: Calculate Damage
        //damage.damageTotal = 0.0f;
        //return damage;
    }
}
 