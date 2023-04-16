using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MultiplayerEntity : MonoBehaviour, IPunObservable
{
    public Player_PlayerController playerController;
    public string uniqueID {  get; private set; }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(uniqueID);
        }
    }

    public bool RegisterUniqueID(string uniqueID)
    {
        this.uniqueID = uniqueID;
        //TODO
        return false;
    }
    private void Start()
    {
        playerController = GetComponent<Player_PlayerController>();
        if (Game_RuntimeData.isMultiplayer)
        {
            playerController.IsInputLocked = true;
            Game_RuntimeData.activePlayers.Add(this);
        }
    }
}
