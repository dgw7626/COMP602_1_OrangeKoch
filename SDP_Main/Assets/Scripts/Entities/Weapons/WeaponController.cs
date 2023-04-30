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
    // Start is called before the first frame update
    void Awake()
    {

        _photonView = GetComponentInParent<PhotonView>();
        _inputHandler = GetComponentInParent<Player_InputManager>();
        _weaponProjectileMananger = GetComponent<WeaponProjectileManager>();
    }
    void Start()
    {
        _isMultiplayer = Game_RuntimeData.isMultiplayer;
      //  PhotonView = FindObj
        //initiating bullet counts.
        _weaponProjectileMananger.InitBullets();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
           // Destroy(GetComponentInChildren<Camera>().gameObject);
        }

        _weaponProjectileMananger.UpdateChildTransform();
     
        
    }
}
