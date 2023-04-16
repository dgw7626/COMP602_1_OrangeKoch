using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Debug.Log("Instantiated Player Controller");
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
        GameObject instance =  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);

        /*Debug.Log("Trying to add ID: " + instance.GetComponent<PhotonView>().Owner.ActorNumber);
        Game_RuntimeData.temp.Add(instance.GetComponent<PhotonView>().Owner.ActorNumber, instance.GetComponent<Player_MultiplayerEntity>());
        Debug.Log("Added ID: " + instance.GetComponent<PhotonView>().Owner.ActorNumber);*/


        int x = PhotonNetwork.LocalPlayer.ActorNumber;
        
        //Game_RuntimeData.activePlayers.Add(instance.GetComponent<Player_MultiplayerEntity>());
       // instance.GetComponent<PhotonView>().Owner.TagObject = instance;
        //instance.GetComponent<Player_MultiplayerEntity>().RegisterUniqueID(instance.GetComponent<PhotonView>().Owner.NickName);
    }

    void Update()
    {
        
    }
}
