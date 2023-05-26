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
        //Calculate Winning Team
        int winningTeam = 0;
        int winningTeamKillCount = m_score.killsPerTeam[0];
        for(int i = 1; i < m_score.numTeams; i++)
        {
            if (m_score.killsPerTeam[i] > winningTeamKillCount)
            {
                winningTeam = i;
                winningTeamKillCount = m_score.killsPerTeam[i];
            }
        }

        //Calculate MVP
        int mvpID = 0;
        int mvpKills = m_score.killsPerPlayer[0];
        for (int i = 1; i < m_score.numPlayers; i++)
        {
            if (m_score.killsPerPlayer[i] > winningTeamKillCount)
            {
                mvpID = i;
                mvpKills = m_score.killsPerTeam[i];
            }
        }

        GameObject tmp;
        for (int i = 0; i < m_score.numPlayers; i++) 
        {
            string winTxt = "";

            if (i == mvpID)
                winTxt += " (MVP!)";

            tmp = Instantiate(textPrefab, m_NameCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("(Team" + m_score.teamNumbersByPlayer[i] + ") " +  "Player" + (i+1) + winTxt);

            tmp = Instantiate(textPrefab, m_KillCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.killsPerPlayer[i]);
            
            tmp = Instantiate(textPrefab, m_DeathCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("" + m_score.deathsPerPlayer[i]);
        }

        for(int i = 0; i < m_score.numTeams; i++)
        {
            string winTxt = "";

            if (i == winningTeam)
                winTxt += "\nWinner!";
            else
                winTxt += "\nLOOSER!";

            tmp = Instantiate(textPrefab, m_TeamNameCollumn.transform);
            tmp.GetComponent<TextMeshProUGUI>().text = ("Team " + (i + 1) + winTxt);

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
