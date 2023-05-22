using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Weapon_ProjectileManager))]
[RequireComponent(typeof(PhotonView))]
public class Weapon_Controller : MonoBehaviour
{
    public PhotonView photonView { get; private set; }
    private Weapon_ProjectileManager _weaponProjectileMananger;
    [SerializeField]
    internal bool isMultiplayer = false;
    void Awake()
    {
        //get the photon component to call the rpc method.
        photonView = GetComponent<PhotonView>();
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    void Start()
    {
        // if its multiplayer instantiate bullet object instances otherwise will be instantiated without photon.
        if (isMultiplayer)
        {
            _weaponProjectileMananger.InitBullets_P();
        }
        else
        {
            _weaponProjectileMananger.InitBullets();
        }
    }
    void Update()
    {
        //if its multiplayer get the camera transform and parent to the child object otherwise it will link to local player object child.
        if (!isMultiplayer)
        {
            _weaponProjectileMananger.UpdateChildTransform();
        }

       if (!photonView.IsMine)
       {
           return;
       }
        photonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
    }

 
}
