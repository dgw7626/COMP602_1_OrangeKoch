using Photon.Pun;
using System.Collections;
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
    public HealthBar healthBar;
    private IEnumerator coroutine;

    /// <summary>
    /// Initializes the player's health.
    /// </summary>
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
        // if (transform.position.y >=  2)
        if (transform.position.y <= -10)
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
                Debug.Log(
                    "The photon view belongs to: "
                        + entity.playerController.photonView.Owner.ActorNumber
                );
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
    /// Decreases the player's health by the specified damage value.
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
    public IEnumerator UpdateUI()
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
}
