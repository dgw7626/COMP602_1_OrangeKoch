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
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// This class is designed to Manage a Room and when changing a scene within Multiplayer
/// </summary>
public class Multiplayer_RoomManager : MonoBehaviourPunCallbacks
{
    public static Multiplayer_RoomManager Instance;

    /// <summary>
    /// This Method Instantiates a Room Manager and ensures only one exists
    /// </summary>
    private void Awake()
    {
        if(Instance) //Checks for other Multiplayer_RoomManager Instances
        {
            Destroy(gameObject); //Ensures there is only one Multiplayer_RoomManager.
                return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    /// <summary>
    /// This method increases the number of scenes loaded
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// This method decreases the number of scenes loaded
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// This method Instantiates a PlayerManager if a GameMap scene is being loaded
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name.Contains("GameMap_"))
        { // This references the Game Scene build index.
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
