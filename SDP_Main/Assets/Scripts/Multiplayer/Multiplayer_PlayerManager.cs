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
using UnityEngine;
using Photon.Pun;
using System.IO;

/// <summary>
/// This class is designed to create a Controller for their personal view within the Multiplayer
/// </summary>
public class Multiplayer_PlayerManager : MonoBehaviour
{
    PhotonView PV;

    /// <summary>
    /// Method called when instantiated and retrieves the Photon View. Before Creating a Controller
    /// </summary>
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Creates a Controller only for the players Photon View
    /// </summary>
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
        Debug.Log("Instantiated Player Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
