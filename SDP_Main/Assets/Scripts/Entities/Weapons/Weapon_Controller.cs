/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Hanul Rheem		            *
 * Student ID: 		20109218		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using UnityEngine;
using Photon.Pun;
//Weapon projectile manager will be instantiated.
[RequireComponent(typeof(Weapon_ProjectileManager))]
///<summary>
/// this is weapon controller script that controls user shooting, reloading and damaging the player instance.
/// </summary>
public class Weapon_Controller : MonoBehaviour
{
    //Photon view object for the weapon controller.
    public PhotonView PhotonView { get; private set; }
    //Intializing wepaon projectile manager for the controller.
    private Weapon_ProjectileManager _weaponProjectileMananger;
    /// <summary>
    ///  Functions to run once when object is instantiated.
    /// </summary>
    void Awake()
    {
        //get the photon component to call the rpc method.
        if(Game_RuntimeData.isMultiplayer)
            PhotonView = GetComponent<PhotonView>();
        //Set weapon projectile manager component to child to current object instance.
        _weaponProjectileMananger = GetComponent<Weapon_ProjectileManager>();
    }
    /// <summary>
    ///  Functions to run once when object is initialized.
    /// </summary>
    void Start()
    {
        // if its multiplayer instantiate bullet object instances otherwise will be instantiated without photon.
        if (Game_RuntimeData.isMultiplayer)
        {
            //intialize bullets if its multiplayer instance
            _weaponProjectileMananger.InitBullets();
        }
        else
        {
            //initialie bullets if its local player instance
            _weaponProjectileMananger.InitBullets();
        }
    }
    /// <summary>
    /// this update method called every 1 per frame.
    /// </summary>
    void Update()
    {
        //if its multiplayer get the camera transform and parent to the child object otherwise it will link to local player object child.
        if (!Game_RuntimeData.isMultiplayer)
        {
            //Update helding gun transform position to the main camera object.
            _weaponProjectileMananger.UpdateChildTransform();
            return;
        }
        //  if the photon view is not mine
       if (!PhotonView.IsMine)
       {
           return;
       }
       // call all the helding gun transform positions to be main camera object.
        PhotonView.RPC(nameof(_weaponProjectileMananger.UpdateChildTransform), RpcTarget.All);
    }
}
