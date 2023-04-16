using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon_ProjectileManager))]
public class Weapon_WeaponController : MonoBehaviour
{
    public Player_InputManager playerInputManager;
    internal Weapon_ProjectileManager weaponProjectileMananger;
    // Start is called before the first frame update
    void Awake()
    {
        playerInputManager = GetComponentInChildren<Player_InputManager>();
        weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
        //initiating bullet counts.
        weaponProjectileMananger.InitBullets();    
    }

    // Update is called once per frame
    void Update()
    {
        weaponProjectileMananger.UpdateChildTransform();
        if (playerInputManager.GetFireInputDown())
        {
            weaponProjectileMananger.InitShoot(Weapon_E_Firetype.SEMI);
        }
    }
}
