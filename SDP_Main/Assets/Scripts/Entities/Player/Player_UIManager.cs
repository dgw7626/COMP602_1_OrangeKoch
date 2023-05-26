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

    private void UpdateTimer()
    {
        int seconds = GameMode_Manager.gameTime;

        int minutes = seconds / 60;
        seconds = seconds % 60;

        string middleStr = ":";
        if (seconds < 10)
            middleStr += "0";

        timerText.text = "" + minutes + middleStr + seconds;
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
        } else
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
