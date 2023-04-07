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


    float m_FootstepDistanceCounter;

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
