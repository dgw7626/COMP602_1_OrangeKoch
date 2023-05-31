using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_RespawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Spawn_point;

  /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Spawn_point.SetActive(false);
    }

}
