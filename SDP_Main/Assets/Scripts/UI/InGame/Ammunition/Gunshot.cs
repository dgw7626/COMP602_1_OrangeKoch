using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gunshot : MonoBehaviour
{
    // Start is called before the first frame update
    public AmmunitionUI ammunitionUI;
    public int currentBullet;
    private bool Gunshootinput;
    public float shootspeed = 0.1f;
    public float fireTimer;
    public int bulletMag = 300;
    public int bulletLeft = 250;
    void Start()
    {
        currentBullet = bulletMag;
   

        ammunitionUI.SetAmmunition(currentBullet,bulletMag);

        
    }

    // Update is called once per frame
    void Update()
    {
        // Gunshootinput = Input.GetMouseButton(0);
        Gunshootinput = Input.GetMouseButtonDown(0);
        if (Gunshootinput)
        {
            Fire();
        }

        if (fireTimer < shootspeed)
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void Fire()
    {
        if (fireTimer < shootspeed || currentBullet <= 0) return;
        currentBullet--;
        ammunitionUI.UpdateUI(currentBullet, bulletMag);
        fireTimer = 0f;
    }


}
