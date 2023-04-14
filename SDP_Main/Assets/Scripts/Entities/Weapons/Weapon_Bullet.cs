using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bullet : Weapon_I_Fireable
{

    public void Fire()
    {
        Debug.Log("happyTooniverse");
    }

    public void Hit()
    {
        throw new System.NotImplementedException();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
