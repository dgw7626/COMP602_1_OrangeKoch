using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update

    public float maxHealth = 100;
    public float currentHealth;
    public float currentUIHealth;
    public float UI_HealthTime = 0.16f;

    public Vector3 respawnPosition;
    public HealthBar healthBar;

    private bool hasBegun = false;
    private bool pvIsMine = false;
    private IEnumerator conroutine;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(!Game_RuntimeData.isMultiplayer)
        {
            respawnPosition = new Vector3(-25, 0, -25);
            conroutine = UpdateUI();
            StartCoroutine(conroutine);
        }
        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //-----------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            s_DamageInfo damageInfo = new s_DamageInfo();
            damageInfo.dmgValue = 10f;

            TakeDamage(damageInfo);
        }
        if (transform.position.y < -20)
        {
            s_DamageInfo damageInfo = new s_DamageInfo();
            damageInfo.dmgValue = 10f;
            Die(damageInfo);
        }
    }
    //-----------------------------------------------------------------
    
    public void Begin(Player_MultiplayerEntity entity)
    {
  
        hasBegun = true;

        if(entity.playerController.photonView.IsMine)
        {
            if(entity.playerController.photonView.IsMine)
            {
                Debug.Log("The photon view belongs to: " + entity.playerController.photonView.Owner.ActorNumber);
                Debug.Log("Local ID: " + PhotonNetwork.LocalPlayer.ActorNumber);
            }
            
            pvIsMine = true;
            //healthBar.gameObject.SetActive(true);
            conroutine = UpdateUI();
            StartCoroutine(conroutine);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(s_DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.dmgValue;
        if(currentHealth <= 0) 
        {
            Die(damageInfo);
        }
        // Debug.Log("My id is:\nI am " + gameObject.GetComponent<Player_PlayerController>().photonView.Owner.ActorNumber.ToString()
        //     + "\nMy health bar is active: " + healthBar.gameObject.activeSelf + "\nPV is mine: " + pvIsMine);
    }

    void Die(s_DamageInfo damageInfo)
    {

        if (!Game_RuntimeData.isMultiplayer)
        {
            SoloRespawn();
        }
        else
        {
            s_DeathInfo deathInfo = new s_DeathInfo();
            //TODO: TEAMS
            deathInfo.killerTeam = 0;
            deathInfo.diedTeam = 0;
            deathInfo.killerId = damageInfo.dmgDealerId;
            deathInfo.diedId = damageInfo.dmgRecievedId;

            gameObject.GetComponent<Player_PlayerController>().photonView.RPC(
                nameof(Player_MultiplayerEntity.OnPlayerKilled), RpcTarget.All, JsonUtility.ToJson(deathInfo));
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

    IEnumerator UpdateUI()
    {
        while(true)
        {
            if(currentUIHealth > currentHealth)
            {
                float speed = 1f;
                if((currentUIHealth - currentHealth) > 20)
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
