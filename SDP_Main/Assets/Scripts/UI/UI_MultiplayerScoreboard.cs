using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the UI logic for the post-game splash screen.
/// It dynamically generates UI elements based on the the number of teams and players.
/// </summary>
public class UI_MultiplayerScoreboard : MonoBehaviour
{
    private List<List<TextMeshProUGUI>> m_UIelementst;
    private s_GameScore m_score;
    private VerticalLayoutGroup m_NameCollumn;
    private VerticalLayoutGroup m_KillCollumn;
    private VerticalLayoutGroup m_DeathCollumn;

    private VerticalLayoutGroup m_TeamNameCollumn;
    private VerticalLayoutGroup m_TeamKillCollumn;
    private VerticalLayoutGroup m_TeamDeathCollumn;

    public GameObject textPrefab;

    /// <summary>
    /// Assigns UI layout groups to member references, and gets the score from RuntimeData
    /// </summary>
    void Start()
    {
        m_NameCollumn = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>();
        m_KillCollumn = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VerticalLayoutGroup>();
        m_DeathCollumn = transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<VerticalLayoutGroup>();
        
        m_TeamNameCollumn = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<VerticalLayoutGroup>();
        m_TeamKillCollumn = transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<VerticalLayoutGroup>();
        m_TeamDeathCollumn = transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<VerticalLayoutGroup>();

        m_score = Game_RuntimeData.gameScore;

        Init();
    }

    /// <summary>
    /// Dynamically instantiate UI elements, assigns text elements a value from the score struct.
    /// </summary>
    private void Init()
    {
        GameObject tmp;
        for (int i = 0; i < m_score.numPlayers; i++) 
        {
            tmp = Instantiate(textPrefab, m_NameCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("Player" + (i+1));

            tmp = Instantiate(textPrefab, m_KillCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.killsPerPlayer[i]);
            
            tmp = Instantiate(textPrefab, m_DeathCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.deathsPerPlayer[i]);
        }

        for(int i = 0; i < m_score.numTeams; i++)
        {
            tmp = Instantiate(textPrefab, m_TeamNameCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("Team " + (i + 1));

            tmp = Instantiate(textPrefab, m_TeamKillCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.killsPerTeam[i]);

            tmp = Instantiate(textPrefab, m_TeamDeathCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.deathsPerTeam[i]);
        }
    }

    /// <summary>
    /// Quit back to the lobby
    /// </summary>
    void Update()
    {
        // Messy hack that breaks all of our conventions
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            Game_GameState.NextScene("Lobby");
        }
    }
}
