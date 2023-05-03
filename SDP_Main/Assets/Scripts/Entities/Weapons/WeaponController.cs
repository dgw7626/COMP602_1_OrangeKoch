using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(WeaponProjectileManager))]
public class WeaponController : MonoBehaviour
{
    private PhotonView _photonView;
    private WeaponProjectileManager _weaponProjectileMananger;
    void Awake()
    {
       
        _photonView = GetComponentInParent<PhotonView>(); 
        _weaponProjectileMananger = GetComponent<WeaponProjectileManager>();
    }
    void Start()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        GuardClause.InspectGuardClauseNullRef<WeaponProjectileManager>(this._weaponProjectileMananger, nameof(this._weaponProjectileMananger));
        _weaponProjectileMananger.InitBullets();    
    }


    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        _weaponProjectileMananger.UpdateChildTransform();
    }
}
