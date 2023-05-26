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

    public HealthBar healthBar;

    private bool hasBegun = false;
    private bool pvIsMine = false;
    private IEnumerator conroutine;
    public void Begin(Player_MultiplayerEntity entity)
    {
        hasBegun = true;

        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

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

    /// <summary>
    /// Subtract hp from the local instance. If hp falls below zero, inform all players that I have died.
    /// Update the UI value.
    /// </summary>
    /// <param name="damageInfo"></param>
    public void TakeDamage(s_DamageInfo damageInfo)
    {
        //Subtract damage
        currentHealth -= damageInfo.dmgValue;
        
        //If died, inform others
        if(currentHealth < 0) 
        {
            s_DeathInfo deathInfo = new s_DeathInfo();
            deathInfo.killerTeam = damageInfo.dmgRecievedTeam;
            deathInfo.diedTeam = damageInfo.dmgDealerTeam;
            deathInfo.killerId = damageInfo.dmgDealerId;
            deathInfo.diedId = damageInfo.dmgRecievedId;

            gameObject.GetComponent<Player_PlayerController>().photonView.RPC(
                nameof(Player_MultiplayerEntity.OnPlayerKilled), RpcTarget.All, JsonUtility.ToJson(deathInfo));

            currentHealth = maxHealth;
        }

        //Tell the UI 
        healthBar.SetHealth(currentHealth);
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
                currentUIHealth -= speed;

                healthBar.SetHealth(currentUIHealth);
            }
            yield return new WaitForSeconds(UI_HealthTime);
        }
        

    }

}
