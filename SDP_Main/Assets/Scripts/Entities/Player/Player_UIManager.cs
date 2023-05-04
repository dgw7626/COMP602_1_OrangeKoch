using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Player_UIManager : MonoBehaviour
{

    public TextMeshProUGUI proximityMuteText;
    public TextMeshProUGUI pushToTalkText;

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

    private void OnQuitGameButtonPressed()
    {
        throw new NotImplementedException();
    }

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

    public void SetPTTtext(string text)
    {
        pushToTalkText.text = text;
    }

}
