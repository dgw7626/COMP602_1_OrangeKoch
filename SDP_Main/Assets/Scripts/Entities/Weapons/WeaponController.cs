using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(WeaponProjectileManager))]
public class WeaponController : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    private WeaponProjectileManager _weaponProjectileMananger;
    void Awake()
    {
       
        PhotonView = GetComponent<PhotonView>();
        _weaponProjectileMananger = GetComponent<WeaponProjectileManager>();
    }
    void Start()
    {
        if (!PhotonView.IsMine)
        {
            return;
        }
        PhotonView.RPC(nameof(_weaponProjectileMananger.InitBullets), RpcTarget.All);
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
