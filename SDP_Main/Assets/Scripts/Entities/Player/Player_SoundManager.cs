using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

            proximityMuteText.enabled = true;
            pushToTalkText.enabled = true;
            proximityVoiceMute = false;
            proximityMuteText = transform.parent.Find("PlayerUI").Find("ProximityMute_Text").GetComponent<TextMeshProUGUI>();
            proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
            pushToTalkText = transform.parent.Find("PlayerUI").Find("PTT_Text").GetComponent<TextMeshProUGUI>();
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
        }
    }

    /// <summary>
    /// Update method to constantly check for changes.
    /// </summary>
    private void Update()
    {
        //Return if not playing multiplayer, such that it is single.
        if (!Game_RuntimeData.isMultiplayer)
            return;

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

    /// <summary>
    /// Toggles the Mute for incoming player voice channel
    /// </summary>
    private void OnMuteButtonPressed()
    {
        // Invert VoiceChat Variable
        proximityVoiceMute = !proximityVoiceMute;
        Debug.Log("Mute Button Pressed!");

        // Create List object reference for list of game objects
        List<GameObject> rootObjects = new List<GameObject>();

        // Set List of gameobjects for the scene
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);


        // Iterate through root objects
        for (int i = 0; i < rootObjects.Count; ++i)
        {
            // Check if object has Player tag
            if (rootObjects[i].tag == "Player")
            {
                // Set player object reference
                GameObject player = rootObjects[i];

                //Set Player Transform reference
                Transform playerTransform = player.transform;

                //Loop through Player Objects
                for (int j = 0; j < playerTransform.childCount; j++)
                {
                    // Check for Child Object with SoundSystem Tag
                    if (playerTransform.GetChild(j).gameObject.tag == "SoundSystem")
                    {
                        // Set the Player Speaker Game Object Reference
                        GameObject speaker = playerTransform.GetChild(j).gameObject;

                        // Toggle the Player Voice Speaker Component Active or Inactive
                        Debug.Log("Player Voice Speaker Toggled");
                        speaker.transform.GetChild(0).GetComponent<Speaker>().enabled = !proximityVoiceMute;
                    }
                }
            }
        }
        // Update VoiceChat UI Display
        proximityMuteText.text = "(Press \"M\") Mute: " + ((proximityVoiceMute) ? "MUTE" : "UNMUTE");
        if(proximityVoiceMute)
        {
            pushToTalkText.text = "PTT: Disabled, Proximity Muted!";
        } else
        {
            pushToTalkText.text = "(Press \"LEFT ALT\") PTT: OFF";
        }
    }
    /// <summary>
    /// Action performed to enable microphone when PTT key is pressed
    /// </summary>
    private void OnPTTButtonPressed()
    {
        if(proximityVoiceMute)
        {
            //Prevent Transmitting Voice
            Debug.Log("Proximity Muted, cannot PTT!");
            return;
        }
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
    /// <summary>
    /// Action performed to disable microphone when PTT key is released
    /// </summary>
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
