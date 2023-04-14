using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Manager : MonoBehaviour
{
    public IgameMode gameMode;
    // Start is called before the first frame update
    void Awake()
    {
        if(Game_RuntimeData.gameMode == null)
        {
            Game_RuntimeData.gameMode = new GameMode_Standard();
        }
        gameMode = Game_RuntimeData.gameMode;

        Invoke("Init", 1);
    }

    void Init()
    {
        gameMode.InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
