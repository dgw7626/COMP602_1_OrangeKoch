using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ProjectileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Weapon_Bullet bullet = new Weapon_Bullet();
        bullet.Fire();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
