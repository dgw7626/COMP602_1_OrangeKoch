using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(WeaponProjectileManager))]
public class WeaponController : MonoBehaviour
{
    private Player_InputManager _inputHandler;
    private WeaponProjectileManager _weaponProjectileMananger;

   [SerializeField] private PhotonView _photonView;
    private bool _isMultiplayer;
    void Awake()
    {
        _photonView = GetComponentInParent<PhotonView>();
        _inputHandler = GetComponentInParent<Player_InputManager>();
        _weaponProjectileMananger = GetComponent<WeaponProjectileManager>();
    }
    void Start()
    {
        _isMultiplayer = Game_RuntimeData.isMultiplayer;
        _weaponProjectileMananger.InitBullets();    
    }


    void Update()
    {
        //dont need to add guard clause on this you only put the one made.
        if (!_photonView.IsMine)
        {
            return;
        }

        _weaponProjectileMananger.UpdateChildTransform();
     
        
    }
}
