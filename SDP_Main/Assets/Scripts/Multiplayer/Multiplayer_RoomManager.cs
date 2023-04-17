using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class Multiplayer_RoomManager : MonoBehaviourPunCallbacks
{

    public static Multiplayer_RoomManager Instance;

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


    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name.Contains("GameMap_"))
        { // This references the Game Scene build index.
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
