using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]
public class Weapon_Controller : MonoBehaviour
{
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponController.cs
    public PhotonView PhotonView { get; private set; }
    private WeaponProjectileManager _weaponProjectileMananger;
    void Awake()
    {
       
        PhotonView = GetComponent<PhotonView>();
        _weaponProjectileMananger = GetComponent<WeaponProjectileManager>();
=======
    private PhotonView _photonView;
    private Weapon_ProjectileManager _weaponProjectileMananger;
    void Awake()
    {
       
        _photonView = GetComponentInParent<PhotonView>(); 
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Controller.cs
    }
    void Start()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponController.cs
        PhotonView.RPC(nameof(_weaponProjectileMananger.InitBullets), RpcTarget.All);
=======
        GuardClause.InspectGuardClauseNullRef<Weapon_ProjectileManager>(this._weaponProjectileMananger, nameof(this._weaponProjectileMananger));
        _weaponProjectileMananger.InitBullets();    
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Controller.cs
    }
    void Update()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
        PhotonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
      //  _weaponProjectileMananger.UpdateChildTransform();
    }

 
}
