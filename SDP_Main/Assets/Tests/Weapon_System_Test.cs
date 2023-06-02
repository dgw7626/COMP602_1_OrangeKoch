using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.IO;

public class Weapon_System_Test : MonoBehaviour
{
    // Weapon bullet, Weapon Projectile Mananger, Weapon Controller, Weapon Infomraiton.
    internal GameObject _player;
    internal Weapon_Bullet _bullet;
    internal Weapon_ProjectileManager _projectileManager;
    internal Weapon_Controller _controller;
    internal Weapon_Info _info;
    [SetUp]
    public void Player_Init()
    {
        this._player = Instantiate(
            Resources.Load(Path.Combine("PhotonPrefabs", "Player")) as GameObject,
            Vector3.zero,
            Quaternion.identity
        );

        if(this._player != null )
        {
            this._player.transform.name = "Test_Player";
            Transform parent = this._player.transform.Find("WeaponHolder");
            DestroyImmediate(parent.GetComponent<Weapon_Controller>());
            DestroyImmediate(parent.GetComponent<Weapon_ProjectileManager>());
            Debug.Log(parent.GetComponent<Weapon_Controller>());
            Debug.Log(parent.transform.name);

            this._controller = parent.gameObject.AddComponent<Weapon_Controller>();
            this._projectileManager = parent.GetComponent<Weapon_ProjectileManager>();
            this._projectileManager.WeaponInfo = ScriptableObject.CreateInstance<Weapon_Info>();
            this._projectileManager.WeaponInfo.BulletCounts = 30;
            this._projectileManager.WeaponInfo.ClipCounts = 10;

            //inser values here
            this._projectileManager.WeaponAmmo = this._projectileManager.WeaponInfo.BulletCounts;
            this._projectileManager.WeaponClip = this._projectileManager.WeaponInfo.ClipCounts;


            Debug.Log(this._projectileManager.WeaponInfo.BulletCounts);
            this._projectileManager.WeaponInfo.BulletShell = Resources.Load(Path.Combine("LocalPrefabs", "Shell")) as GameObject;
            this._projectileManager.WeaponInfo.BulletTrace = Resources.Load(Path.Combine("LocalPrefabs", "BulletTracer")) as TrailRenderer;  //trailrendder
            this._projectileManager.WeaponInfo.MuzzleFlash = Resources.Load(Path.Combine("LocalPrefabs", "MuzzleFlash")) as ParticleSystem;  //particleystem
            Debug.Log(this._projectileManager.WeaponInfo.BulletShell.transform.name);
            this._projectileManager.WeaponInfo.WeaponName = "Glock"; //
            this._projectileManager.WeaponInfo.ReloadEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test","AK47")) as AudioClip;//audio clip
            this._projectileManager.WeaponInfo.ShootEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test","AK47")) as AudioClip; //audio clip
            this._projectileManager.WeaponInfo.HitEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test","AK47")) as AudioClip;// audio clip
            Debug.Log(this._projectileManager.WeaponInfo.HitEffect.name);
                                                                                                                                   // you have to set everything manually here fuck!

            //instanitate bullets
            this._projectileManager.InitBullets();
            //   this._controller.gameObject.AddComponent<Weapon_Controller>();
            //   this._projectileManager.gameObject.GetComponent<Weapon_ProjectileManager>().WeaponInfo = _inf
            Debug.Log(this._projectileManager.LocalBullets) ;

        }
        return;
    }
    [Test]
    public void Test_SetDamageValueTo10()
    {
 
    }
    [Test]
    public void Test_SetBulletIndexTo5()
    {

    }
    [Test]
    public void Test_CheckVectorVFX()
    {
        
        
        
    }
}
