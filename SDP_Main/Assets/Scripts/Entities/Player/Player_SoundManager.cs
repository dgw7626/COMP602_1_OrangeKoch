using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        proximityVoiceMute = false;
    }

    private void Update()
    {
        if (transform.parent.GetComponent<Player_InputManager>().GetVoiceMuteButtonIsPressed())
            OnMuteButtonPressed();
    }

    private void OnMuteButtonPressed()
    {
        proximityVoiceMute = !proximityVoiceMute;

        transform.GetChild(0).GetComponent<AudioSource>().mute = proximityVoiceMute;
        Debug.Log(transform.GetChild(0));
        Debug.Log(transform.GetChild(0).GetComponent<AudioSource>());
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

    internal void PLayLand()
    {
        AudioSource.PlayOneShot(LandSfx);
    }
}
