using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxHealth = 100;
    public float currentHealth;
    public float currentUIHealth;
    public float UI_HealthTime = 0.16f;

    public HealthBar healthBar;

    private IEnumerator conroutine;
    void Start()
    {
        conroutine= UpdateUI();
        StartCoroutine(conroutine);

        currentHealth = maxHealth;
        currentUIHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
