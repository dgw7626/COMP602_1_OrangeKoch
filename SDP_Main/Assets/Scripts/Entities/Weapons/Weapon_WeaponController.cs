using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]
public class Weapon_WeaponController : MonoBehaviour
{
    public Player_InputManager playerInputManager;
    internal Weapon_ProjectileManager weaponProjectileMananger;

    [SerializeField] internal PhotonView PhotonView;
    internal bool IsMultiplayer;
    // Start is called before the first frame update
    void Awake()
    {
        

        playerInputManager = GetComponentInChildren<Player_InputManager>();
        weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
        IsMultiplayer = Game_RuntimeData.isMultiplayer;
      //  PhotonView = FindObj
        //initiating bullet counts.
        weaponProjectileMananger.InitBullets();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonView.IsMine)
        {
            return;
           // Destroy(GetComponentInChildren<Camera>().gameObject);
        }

        weaponProjectileMananger.UpdateChildTransform();
        if (playerInputManager.GetFireInputDown())
        {
            weaponProjectileMananger.InitShoot(Weapon_E_Firetype.SEMI);
        }
        
    }
}
