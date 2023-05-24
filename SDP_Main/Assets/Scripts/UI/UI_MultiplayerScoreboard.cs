using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MultiplayerScoreboard : MonoBehaviour
{
    private List<List<TextMeshProUGUI>> m_UIelementst;
    private s_GameScore m_score;
    private VerticalLayoutGroup m_NameCollumn;
    private VerticalLayoutGroup m_KillCollumn;
    private VerticalLayoutGroup m_DeathCollumn;
    // Start is called before the first frame update
    void Start()
    {
        m_NameCollumn = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>();
        m_KillCollumn = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VerticalLayoutGroup>();
        m_DeathCollumn = transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<VerticalLayoutGroup>();

        m_score = Game_RuntimeData.gameScore;

        Init();
    }

    private void Init()
    {
        for(int i = 0; i < m_score.numPlayers; i++) 
        {
            TextMeshProUGUI tmp = Instantiate(new TextMeshProUGUI(), m_NameCollumn.transform);
            tmp.text = ("Player" + i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
