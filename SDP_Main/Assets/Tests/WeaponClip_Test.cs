using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System;

public class WeaponClip_Test
{
    private Weapon_ProjectileManager _projectileManager;
    private Weapon_Controller _weaponController;
    private AmmunitionUI _ammunitionUI;
    public Weapon_Info weaponInfo;
    [SetUp]
    public void Init()
    {
        _projectileManager = new GameObject().AddComponent<Weapon_ProjectileManager>();
        _weaponController = _projectileManager.gameObject.AddComponent<Weapon_Controller>();
        _projectileManager.firePos = new GameObject().transform;
        _ammunitionUI = new GameObject().AddComponent<AmmunitionUI>();
        _ammunitionUI.Text = new GameObject().AddComponent<Text>();
        weaponInfo = Resources.Load<Weapon_Info>("WeaponData/Glock_modern");
        _projectileManager._weaponInfo = weaponInfo;
        _projectileManager._weaponAmmo = this.weaponInfo.BulletCounts;
        _projectileManager._weaponClip = this.weaponInfo.ClipCounts;
        _weaponController.isMultiplayer = false;
        _projectileManager._ammunitionUI = this._ammunitionUI;
        _projectileManager.firePos.name = "firePos";
        _projectileManager._weaponController = this._weaponController;
        _projectileManager.InitBullets();
        Debug.Log("PROJECT:" +_projectileManager._weaponInfo.ShootEffect);
        Debug.Log(_ammunitionUI);
        Debug.Log(_weaponController);
        Debug.Log(_projectileManager);
    }

    

    [Test]
    public void GetShoot_DecreasesAmmoAndFiresBullet()
    {
        // Arrange
        //enable false to make the player local.

        //Initiate bullet instances.

        // var bulletComponent = bulletPrefab.AddComponent<Weapon_Bullet>();

        // Act
        _projectileManager.InitShoot(Weapon_Firetype.Semi);
        Debug.Log(_projectileManager._weaponAmmo);
        // Assert
        Assert.AreEqual(0, _projectileManager._weaponAmmo);
       // Assert.IsTrue(bulletComponent.gameObject.activeSelf);
    }

    [Test]
    public void Reload_RefillsAmmoAndDecreasesClip()
    {
        // Arrange
        var reloadingComponent = new GameObject().AddComponent<Weapon_ProjectileManager>();
        reloadingComponent._weaponClip = 2;
        reloadingComponent._weaponAmmo = 10;
        reloadingComponent._weaponInfo = ScriptableObject.CreateInstance<Weapon_Info>();
        reloadingComponent._weaponInfo.BulletCounts = 15;
        var ammunitionUI = new GameObject().AddComponent<AmmunitionUI>();
        reloadingComponent._ammunitionUI = ammunitionUI;

        // Act
        reloadingComponent.Reload();

        // Assert
        Assert.AreEqual(15, reloadingComponent._weaponAmmo);
        Assert.AreEqual(1, reloadingComponent._weaponClip);
        //Assert.AreEqual(15, ammunitionUI.GetAmmoCount());
        //Assert.AreEqual(1, ammunitionUI.GetClipCount());
    }


    //[Test]
    //public void GetShoot_DecreasesAmmoAndActivatesBullet()
    //{
    //    // Arrange
    //    var weaponProjectileManager = new GameObject().AddComponent<WeaponProjectileManager>();
    //    weaponProjectileManager._localBullets = new System.Collections.Generic.List<WeaponBullet>();
    //    var bullet = new GameObject().AddComponent<WeaponBullet>();
    //    bullet.gameObject.SetActive(false);
    //    weaponProjectileManager._localBullets.Add(bullet);
    //    weaponProjectileManager._weaponAmmo = 10;
    //    var expectedAmmo = weaponProjectileManager._weaponAmmo - 1;

    //    // Act
    //    weaponProjectileManager.GetShoot();

    //    // Assert
    //    Assert.AreEqual(expectedAmmo, weaponProjectileManager._weaponAmmo);
    //    Assert.IsTrue(bullet.gameObject.activeSelf);
    //}

}
