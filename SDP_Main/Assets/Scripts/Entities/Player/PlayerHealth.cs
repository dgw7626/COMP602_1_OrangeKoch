using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
   

    public float maxHealth = 100;
    public float currentHealth;
    public float currentUIHealth;
    public float UI_HealthTime = 0.16f;
    public float damage = 25f;

    public HealthBar healthBar;
    public Vector3 respawnPosition;
    // Determine the mutiplayer
    private bool IsMultiplayer;

    private IEnumerator conroutine;

    GameMode_Standard gameMode_Standard;

    void Start()
    {
        IsMultiplayer = PhotonNetwork.IsConnected;

        respawnPosition = new Vector3(-25, 0, -25);

        conroutine = UpdateUI();
        StartCoroutine(conroutine);

        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(damage);
        }
        if (transform.position.y < -20)
        {
            Die();
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
           
        }
    }

    IEnumerator UpdateUI()
    {
        while (true)
        {
            if (currentUIHealth > currentHealth)
            {
                float speed = 1f;
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
        // /

    }

    void Die()
    {

        if (!IsMultiplayer)
        {
            SoloRespawn();
        }
        else if(!IsMultiplayer)
        {
            if (gameMode_Standard != null)
            {
                // Call the OnPlayerKilled() method of the GameMode_Standard object,
                // passing the Player_MultiplayerEntity object representing the killed player
            }
            else
            {
                Debug.LogWarning("Unable to find GameMode_Standard object.");
            }
        }
    }
    void SoloRespawn()
    {
        // Reset
        gameObject.transform.position = respawnPosition;
        Debug.Log(transform.position);
        currentHealth = maxHealth;
        // Weird hack, don't know why this works
        Physics.SyncTransforms();
        // Update UI
        currentUIHealth = maxHealth;
        healthBar.SetHealth(currentUIHealth);
    }
}
