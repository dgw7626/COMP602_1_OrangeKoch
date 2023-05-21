using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]
[RequireComponent(typeof(PhotonView))]
public class Weapon_Controller : MonoBehaviour
{
    public PhotonView photonView { get; private set; }
    private Weapon_ProjectileManager _weaponProjectileMananger;
    internal bool hasInitiated = false;
    internal string userName;
    void Awake()
    {

        photonView = GetComponent<PhotonView>();
        userName = PhotonNetwork.NickName;
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
        //Do not add PhotonView ismine.
        _weaponProjectileMananger.InitBullets_P();
       //_weaponProjectileMananger.InitBullets();
    }
    void Update()
    {
       if (!photonView.IsMine)
       {
           return;
       }
        photonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
        //_weaponProjectileMananger.UpdateChildTransform();
    }

 
}
