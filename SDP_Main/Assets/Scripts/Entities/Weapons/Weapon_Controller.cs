using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]

public class Weapon_Controller : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    private Weapon_ProjectileManager _weaponProjectileMananger;
    void Awake()
    {
       
        PhotonView = GetComponent<PhotonView>();
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
      // if (!PhotonView.IsMine)
      // {
      //     return;
      // }
        //   PhotonView.RPC(nameof(_weaponProjectileMananger.InitBullets), RpcTarget.All);
        _weaponProjectileMananger.InitBullets();
    }
    void Update()
    {
      // if (!PhotonView.IsMine)
      // {
      //     return;
      // }
      //  PhotonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
        _weaponProjectileMananger.UpdateChildTransform();
    }

 
}
