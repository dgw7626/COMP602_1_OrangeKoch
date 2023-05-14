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

    public void TakeDamage(s_DamageInfo damageInfo)
    {
        Debug.Log("===============================================================\n=============================================================");
        currentHealth -= damageInfo.dmgValue;
        //if (healthBar.gameObject.activeSelf)
        {
            healthBar.SetHealth(currentHealth);
            Debug.Log("My id is:\nI am " + gameObject.GetComponent<Player_PlayerController>().photonView.Owner.ActorNumber.ToString()
                + "\nMy health bar is active: " + healthBar.gameObject.activeSelf + "\nPV is mine: " + pvIsMine);
        }

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
