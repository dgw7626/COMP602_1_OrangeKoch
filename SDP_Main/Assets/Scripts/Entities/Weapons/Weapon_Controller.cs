using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]
public class Weapon_Controller : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    private Weapon_ProjectileManager _weaponProjectileMananger;
    void Awake()
    {
        //get the photon component to call the rpc method.
        if(Game_RuntimeData.isMultiplayer)
            PhotonView = GetComponent<PhotonView>();
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
        // if its multiplayer instantiate bullet object instances otherwise will be instantiated without photon.
        if (Game_RuntimeData.isMultiplayer)
        {
            _weaponProjectileMananger.InitBullets();
        }
        else
        {
            _weaponProjectileMananger.InitBullets();
        }
    }
    void Update()
    {
        //if its multiplayer get the camera transform and parent to the child object otherwise it will link to local player object child.
        if (!Game_RuntimeData.isMultiplayer)
        {
            _weaponProjectileMananger.UpdateChildTransform();
            return;
        }

       if (!PhotonView.IsMine)
       {
           return;
       }
        PhotonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
    }

 
}
