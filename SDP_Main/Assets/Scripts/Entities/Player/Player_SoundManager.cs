using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_SoundManager : MonoBehaviour
{
    [Tooltip("Audio source for footsteps, jump, etc...")]
    public AudioSource AudioSource;

    [Header("Audio")]
    [Tooltip("Amount of footstep sounds played when moving one meter")]
    public float FootstepSfxFrequency = 1f;

    [Tooltip("Amount of footstep sounds played when moving one meter while sprinting")]
    public float FootstepSfxFrequencyWhileSprinting = 1f;

    [Tooltip("Sound played for footsteps")]
    public AudioClip FootstepSfx;

    [Tooltip("Sound played when jumping")] public AudioClip JumpSfx;
    [Tooltip("Sound played when landing")] public AudioClip LandSfx;

    [Tooltip("Sound played when taking damage froma fall")]
    public AudioClip FallDamageSfx;


    public bool proximityVoiceMute;

    float m_FootstepDistanceCounter;

    public TextMeshProUGUI proximityMuteText;
    public TextMeshProUGUI pushToTalkText;
    private void Awake()
    {
        //Do not load if the instance does not belong to me
        if (!transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            return;
        }

        //Load if the instance belongs to me
        if (transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            //Set up VoiceChat and VoiceChat Display settings
            proximityMuteText.enabled = true;
            pushToTalkText.enabled = true;
            proximityVoiceMute = false;
            proximityMuteText = transform.parent.Find("PlayerUI").Find("ProximityMute_Text").GetComponent<TextMeshProUGUI>();
            proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
            pushToTalkText = transform.parent.Find("PlayerUI").Find("PTT_Text").GetComponent<TextMeshProUGUI>();
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
        }
    }

        private void Update()
    {
        //Do not load if the instance does not belong to me
        if (!transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            return;
        }

        //Check for Mute Button being pressed
        if (transform.parent.GetComponent<Player_InputManager>().GetVoiceMuteButtonIsPressed())
            OnMuteButtonPressed();

        //Check for Push to talk button being pressed
        if (transform.parent.GetComponent<Player_InputManager>().GetPTTButtonIsPressed())
        {
            //Start Transmitting Voice
            OnPTTButtonPressed();
        } else if(!transform.parent.GetComponent<Player_InputManager>().GetPTTButtonIsPressed() &&
            transform.GetChild(0).GetComponent<Recorder>().TransmitEnabled)
        {
            //Finish Transmitting Voice
            OnPTTButtonReleased();
        }



    }

    private void OnMuteButtonPressed()
    {
        proximityVoiceMute = !proximityVoiceMute;
        Debug.Log("Mute Button Pressed!");
        transform.GetChild(0).GetComponent<AudioSource>().mute = proximityVoiceMute;
        //transform.GetChild(0).GetComponent<Speaker>().enabled = !proximityVoiceMute;
        proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
    }
    private void OnPTTButtonPressed()
    {
        if (!transform.GetChild(0).GetComponent<Recorder>().TransmitEnabled)
        {
            //Start Transmitting Voice
            Debug.Log("PTT Button Pressed!");
            transform.GetChild(0).GetComponent<Recorder>().TransmitEnabled = true;
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: ON";
        } else
        {
            //Continue Transmitting Voice
        }
    }
    private void OnPTTButtonReleased()
    {
        Debug.Log("PTT Button Released!");
        transform.GetChild(0).GetComponent<Recorder>().TransmitEnabled = false;
        pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
    }

    internal void PlayFallDamage()
    {
        //AudioSource.PlayOneShot(FallDamageSfx);
    }

    internal void PlayFootstep(bool isSprinting, float magnitude)
    {
        float chosenFootstepSfxFrequency =
                    (isSprinting ? FootstepSfxFrequencyWhileSprinting : FootstepSfxFrequency);
        if (m_FootstepDistanceCounter >= 1f / chosenFootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            AudioSource.PlayOneShot(FootstepSfx);
        }


        // keep track of distance traveled for footsteps sound
        m_FootstepDistanceCounter += magnitude * Time.deltaTime;
    }

    internal void PlayJump()
    {
        //AudioSource.PlayOneShot(JumpSfx);
    }

    internal void PlayLand()
    {
        AudioSource.PlayOneShot(LandSfx);
    }
}
