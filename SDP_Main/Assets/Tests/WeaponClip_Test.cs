using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;

public class WeaponClip_Test
{
    [Test]
    public void getshoot_1()
    {
        // arrange
        var player = new GameObject().AddComponent<Weapon_ProjectileManager>();

        player.WeaponAmmo = 10;

        // act
        //player.GetShoot_1();

        // assert
        Assert.AreEqual(9, player.WeaponAmmo);
        // additional assertions can be added based on the expected behavior
    }

    [Test]
    public void GetShoot_DecreasesAmmoAndFiresBullet()
    {
        // Arrange
        var shootingComponent = new GameObject().AddComponent<Weapon_ProjectileManager>();
        shootingComponent.WeaponAmmo = 3;
        var bulletPrefab = new GameObject();
       // var bulletComponent = bulletPrefab.AddComponent<Weapon_Bullet>();

        // Act
        shootingComponent.GetShoot();

        // Assert
        Assert.AreEqual(2, shootingComponent.WeaponAmmo);
       // Assert.IsTrue(bulletComponent.gameObject.activeSelf);
    }

    [Test]
    public void Reload_RefillsAmmoAndDecreasesClip()
    {
        // Arrange
        var reloadingComponent = new GameObject().AddComponent<Weapon_ProjectileManager>();
        reloadingComponent.WeaponClip = 2;
        reloadingComponent.WeaponAmmo = 10;
        reloadingComponent.WeaponInfo = ScriptableObject.CreateInstance<Weapon_Info>();
        reloadingComponent.WeaponInfo.BulletCounts = 15;
        var ammunitionUI = new GameObject().AddComponent<AmmunitionUI>();
        reloadingComponent.AmmunitionUI = ammunitionUI;

        // Act
        reloadingComponent.Reload();

        // Assert
        Assert.AreEqual(15, reloadingComponent.WeaponAmmo);
        Assert.AreEqual(1, reloadingComponent.WeaponClip);
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
