using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Multiplayer_PlayerManager : MonoBehaviour
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

    /// <summary>
    /// This is how the local player is instantiate. Other players are automatically instantiated by Photon as clones of this object. 
    /// </summary>
    void CreateController()
    {
        // Game_RespawnManager respawnManager = Game_RespawnManager.Instance;
        // Transform spawnPoint = Game_RespawnManager.spawnpoints();
        Transform spawnPoint = SpawnManager.Instance.GetSpawnpoint();

        Debug.Log("Instantiated Player Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.position, spawnPoint.rotation);
    }

    void Update()
    {
        
    }
}
