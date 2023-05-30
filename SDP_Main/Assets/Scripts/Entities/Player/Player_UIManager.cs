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
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is designed to Manage Objects on the Players Interface within the Game
/// </summary>
public class Player_UIManager : MonoBehaviour
{

    public TextMeshProUGUI proximityMuteText;
    public TextMeshProUGUI pushToTalkText;
    public TextMeshProUGUI timerText;
    private Color orangeColor = new Color(1f, 0.65f, 0f); //Create orange as it does not exist by default
    private int redAlertThreshold = 5; // Used for CountdownTimer
    private int orangeAlertThreshold = 15; // Used for CountdownTimer
    private int yellowAlertThreshold = 30; // Used for CountdownTimer
    private bool isRedAlert = false;

    /// <summary>
    /// Functions to run once, when object is instantiated
    /// </summary>
    private void Awake()
    {
        //Return if not playing multiplayer, such that it is single.
        if (!Game_RuntimeData.isMultiplayer)
            return;


        //Do not load if the instance does not belong to me
        if (!transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            return;
        }

        //Load if the instance belongs to me
        if (transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            timerText.text = "";
            timerText.enabled = true;
            PreLoadVoiceUI();
        }
    }

    /// <summary>
    /// Update method to constantly check for changes.
    /// </summary>
    private void Update()
    {
        //Check for Mute Button being pressed
        if (transform.parent.GetComponent<Player_InputManager>().GetQuitGameButtonIsPressed())
            OnQuitGameButtonPressed();

        //Return if not playing multiplayer, such that it is single.
        if (!Game_RuntimeData.isMultiplayer)
            return;

        //Do not load if the instance does not belong to me
        if (!transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            return;
        }

    }

    /// <summary>
    /// Update method to constantly check for changes at a slower rate.
    /// </summary>
    private void FixedUpdate()
    {
        if (Game_RuntimeData.isMultiplayer && !transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            return;
        }
        UpdateTimer();
    }

    /// <summary>
    /// Method that updates the gametimer that is displayed to players.
    /// </summary>
    private void UpdateTimer()
    {
        int seconds = GameMode_Manager.gameTime;
        if (seconds < 0)
        {
            return;
        }
        TimerColorDecider(seconds);

        int minutes = seconds / 60;
        seconds = seconds % 60;

        string middleStr = ":";
        if (seconds < 10)
            middleStr += "0";

        timerText.text = "" + minutes + middleStr + seconds;
    }

    /// <summary>
    /// Decides the countdown timer based on value
    /// </summary>
    private void TimerColorDecider(int seconds)
    {
        // Prevents change on color decider if game has just started
        if (isRedAlert || seconds <= 0)
        {
            if (seconds <= 0)
                isRedAlert = false;
            return;
        }

        if (seconds > yellowAlertThreshold)
        {
            SetTimerColor(Color.green);
            return;
        }
        else if (yellowAlertThreshold >= seconds && seconds > orangeAlertThreshold)
        {
            SetTimerColor(Color.yellow);
        }
        else if (orangeAlertThreshold >= seconds && seconds > redAlertThreshold)
        {
            SetTimerColor(orangeColor);
        }
        else if (redAlertThreshold >= seconds && seconds > 0)
        {
            if (!isRedAlert)
            {
                //Call coroutine to display flash)
                StartCoroutine(RedAlertCountdownTimerFlash());
            }
        }
        else
        {
            SetTimerColor(Color.white);
        }
    }

    /// <summary>
    /// Method to Set the colour of the Countdown timer
    /// </summary>
    private void SetTimerColor(Color color)
    {
        if (color != null)
        {
            timerText.color = color;
        }
    }

    /// <summary>
    /// Coroutine to flash during RedAlert countdown timer final seconds
    /// </summary>
    public IEnumerator RedAlertCountdownTimerFlash()
    {
        //Set red alert to true so the Coroutine is only called once
        isRedAlert = true;
        while (isRedAlert)
        {
            SetTimerColor(Color.red);
            yield return new WaitForSeconds(0.5f);
            SetTimerColor(Color.white);
            yield return new WaitForSeconds(0.5f);
        }

    }

    /// <summary>
    /// Method to Quit the game back to Menu.
    /// </summary>
    private void OnQuitGameButtonPressed()
    {
        Debug.Log("Player quit game.");
        if (Game_RuntimeData.isMultiplayer)
        {
            Game_RuntimeData.gameMode_Manager.QuitMultiplayer();
        }
        else
        {
            Game_RuntimeData.gameMode_Manager.QuitSinglePlayer();
        }
    }

    /// <summary>
    /// Preload one instance of Voice UI for my view
    /// </summary>
    private void PreLoadVoiceUI()
    {
        if (transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            bool proximityVoiceMute = false;
            transform.parent.Find("SoundSystem").GetComponent<Player_SoundManager>().proximityVoiceMute = proximityVoiceMute;
            proximityMuteText = transform.Find("ProximityMute_Text").GetComponent<TextMeshProUGUI>();
            pushToTalkText = transform.Find("PTT_Text").GetComponent<TextMeshProUGUI>();

            proximityMuteText.enabled = true;
            pushToTalkText.enabled = true;
            proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
        }
    }

    /// <summary>
    /// Update the Voice UI when Proximity Voice Mute is Toggled.
    /// </summary>
    public void UpdateVoiceChatUI(bool proximityVoiceMute)
    {
        // Update VoiceChat UI Display
        proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
        if (proximityVoiceMute)
        {
            pushToTalkText.text = "PTT: Disabled, Proximity Muted!";
        }
        else
        {
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
        }
    }

    /// <summary>
    /// Set the Push to Talk VoiceUI text
    /// </summary>
    public void SetPTTtext(string text)
    {
        pushToTalkText.text = text;
    }

}
