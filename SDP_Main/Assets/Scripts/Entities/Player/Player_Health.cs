using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour, IDamageable
{
    // Variables
    public float maxHealth = 100;
    public float currentHealth;
    public float currentUIHealth;
    public float UI_HealthTime = 0.16f;

    public Vector3 respawnPosition;
    public HealthBar healthBar;

    private bool hasBegun = false;
    private bool pvIsMine = false;
    private IEnumerator coroutine;

    void Start()
    {
        // Check if not in multiplayer mode
        if (!Game_RuntimeData.isMultiplayer)
        {
            // Set respawn position for single player mode
            respawnPosition = new Vector3(-25, 0, -25);
            coroutine = UpdateUI();
            StartCoroutine(coroutine);
        }

        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // Simulate damage on key press
        if (Input.GetKeyDown(KeyCode.B))
        {
            s_DamageInfo damageInfo = new s_DamageInfo();
            damageInfo.dmgValue = 10f;
            TakeDamage(damageInfo);
        }

        // Check if player falls below a certain height and cause damage
        if (transform.position.y < -20)
        {
            s_DamageInfo damageInfo = new s_DamageInfo();
            damageInfo.dmgValue = 10f;
            Die(damageInfo);
        }
    }
    /// <summary>
    /// When game start in multiplayer mode.
    /// </summary>
    public void Begin(Player_MultiplayerEntity entity)
    {
        hasBegun = true;

        if (entity.playerController.photonView.IsMine)
        {
            // Check if the PhotonView is owned by the local player
            if (entity.playerController.photonView.IsMine)
            {
                Debug.Log(
                    "The photon view belongs to: "
                        + entity.playerController.photonView.Owner.ActorNumber
                );
                Debug.Log("Local ID: " + PhotonNetwork.LocalPlayer.ActorNumber);
            }

            pvIsMine = true;
            coroutine = UpdateUI();
            StartCoroutine(coroutine);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Health decrease when get damage
    /// </summary>
    public void TakeDamage(s_DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.dmgValue;

        // Check if health reaches zero or below and trigger death
        if (currentHealth <= 0)
        {
            Die(damageInfo);
        }
    }
    /// <summary>
    /// Player died and call Respawn.
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
            s_DeathInfo deathInfo = new s_DeathInfo();
            deathInfo.killerTeam = 0;
            deathInfo.diedTeam = 0;
            deathInfo.killerId = damageInfo.dmgDealerId;
            deathInfo.diedId = damageInfo.dmgRecievedId;
            
            // Call OnPlayerKilled method on the networked player
            gameObject
                .GetComponent<Player_PlayerController>()
                .photonView.RPC(
                    nameof(Player_MultiplayerEntity.OnPlayerKilled),
                    RpcTarget.All,
                    JsonUtility.ToJson(deathInfo)
                );
        }
    }

    /// <summary>
    /// Respawn method for single player mode
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
    /// Coroutine to update UI over time
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

                Debug.Log("UI changing");
                currentUIHealth -= speed;
                healthBar.SetHealth(currentUIHealth);
            }

            yield return new WaitForSeconds(UI_HealthTime);
        }
    }
}
