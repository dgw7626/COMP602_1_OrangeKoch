using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the health of the player character.
/// </summary>
public class Player_Health : MonoBehaviour, IDamageable
{
    // Variables
    public float maxHealth = 100;
    public float currentHealth;
    public float currentUIHealth;
    public float UI_HealthTime = 0.16f;

    public Vector3 respawnPosition;
    public PlayerUI_HealthBar healthBar;
    private IEnumerator coroutine;

    public bool IsDead;
    /// <summary>
    /// Initializes the player's health.
    /// </summary>
    void Start()
    {
        IsDead = false;
        // Check if not in multiplayer mode
        if (!Game_RuntimeData.isMultiplayer)
        {
            // Set respawn position for single player mode
            respawnPosition = new Vector3(-25, 0, -25);
            coroutine = UpdateUI();
            StartCoroutine(coroutine);
        }
        //set initial player health and UI to max.
        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    /// <summary>
    /// Handles the player's input and checks for death conditions.
    /// </summary>
    void Update()
    {
        // Check if player falls below a certain height and cause damage
        if (transform.position.y < -20)
        {
            s_DamageInfo damageInfo = new s_DamageInfo();
            damageInfo.dmgValue = 10f;
            Die(damageInfo);
        }
    }

    /// <summary>
    /// Begins the player's health management in multiplayer mode.
    /// </summary>
    public void Begin(Player_MultiplayerEntity entity)
    {

        if (entity.playerController.photonView.IsMine)
        {
            // Check if the PhotonView is owned by the local player
            if (entity.playerController.photonView.IsMine)
            {
                Debug.Log("The photon view belongs to: " + entity.playerController.photonView.Owner.ActorNumber);
                Debug.Log("Local ID: " + PhotonNetwork.LocalPlayer.ActorNumber);
            }

            // Enable health UI
            coroutine = UpdateUI();
            StartCoroutine(coroutine);
        }
        else
        {
            // Disable health UI for remote players
            healthBar.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Subtract hp from the local instance. If hp falls below zero, inform all players that I have died.
    /// Update the UI value.
    /// </summary>
    /// <param name="damageInfo"></param>
    public void TakeDamage(s_DamageInfo damageInfo)
    {
        if (Game_RuntimeData.isMultiplayer && !Game_RuntimeData.thisMachinesPlayersPhotonView.IsMine)
            return;
        //Subtract damage
        currentHealth -= damageInfo.dmgValue;

        // Check if health reaches zero or below and trigger death
        Debug.Log("Current health is: " +  currentHealth + " and IsDead: " + IsDead);
        if (currentHealth <= 0 && !IsDead)
        {
            Debug.Log("I am dead so I die now");
            Die(damageInfo);
        }
    }

    /// <summary>
    /// Handles the player's death.
    /// </summary>
    void Die(s_DamageInfo damageInfo)
    {
        // Check if not in multiplayer mode
        if (!Game_RuntimeData.isMultiplayer)
        {
            SoloRespawn();
        }
        else
        {
            PhotonView pv = null;
            foreach(KeyValuePair<int, Player_MultiplayerEntity> kvp in Game_RuntimeData.activePlayers)
            {
                if(kvp.Key == damageInfo.dmgRecievedId)
                {
                    pv = kvp.Value.playerController.photonView;
                    break;
                }
            }
            if(pv == null)
            {
                Debug.LogError("NULL photon view found on player killed");
            }

            if(PhotonNetwork.LocalPlayer.ActorNumber != pv.Owner.ActorNumber)
            {
                Debug.LogError("Trying to kill a clone!");
                return;
            }

            s_DeathInfo deathInfo = new s_DeathInfo();
            deathInfo.killerTeam = damageInfo.dmgDealerTeam;
            deathInfo.diedTeam = damageInfo.dmgRecievedTeam;
            deathInfo.killerId = damageInfo.dmgDealerId;
            deathInfo.diedId = damageInfo.dmgRecievedId;

            string json = JsonUtility.ToJson(deathInfo);
            gameObject.GetComponent<Player_PlayerController>().photonView.RPC(
            nameof(Player_MultiplayerEntity.OnPlayerKilled), RpcTarget.All, json);
            IsDead = true;
            Respawn();
        }
    }

    /// <summary>
    /// Respawns the player character in single player mode.
    /// </summary>
    void SoloRespawn()
    {
        // Reset player position to the respawn position
        gameObject.transform.position = respawnPosition;
        Debug.Log(transform.position);

        // Reset health to maximum
        currentHealth = maxHealth;

        // Weird hack, don't know why this works
        Physics.SyncTransforms();

        // Update UI
        currentUIHealth = maxHealth;
        healthBar.SetHealth(currentUIHealth);
    }

    /// <summary>
    /// Updates the UI representing the player's health over time.
    /// </summary>
    IEnumerator UpdateUI()
    {
        while (true)
        {
            if (currentUIHealth > currentHealth)
            {
                float speed = 1f;

                // Increase UI update speed if the health difference is large
                if ((currentUIHealth - currentHealth) > 20)
                {
                    speed = 8f;
                }
                //Update the health bar
                currentUIHealth -= speed;
                healthBar.SetHealth(currentUIHealth);
            }

            yield return new WaitForSeconds(UI_HealthTime);
        }
    }

    public void Respawn()
    {
        IsDead = false;
        currentHealth = maxHealth;
        currentUIHealth = currentHealth;
        healthBar.SetHealth(currentUIHealth);
    }
}
