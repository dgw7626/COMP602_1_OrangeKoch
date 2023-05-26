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
        var player = new GameObject().AddComponent<WeaponProjectileManager>();

        player._weaponAmmo = 10;

        // act
        player.GetShoot_1();

        // assert
        Assert.AreEqual(9, player._weaponAmmo);
        // additional assertions can be added based on the expected behavior
    }

    [Test]
    public void GetShoot_DecreasesAmmoAndFiresBullet()
    {
        // Arrange
        var shootingComponent = new GameObject().AddComponent<WeaponProjectileManager>();
        shootingComponent._weaponAmmo = 3;
        var bulletPrefab = new GameObject();
        var bulletComponent = bulletPrefab.AddComponent<WeaponBullet>();

        // Act
        shootingComponent.GetShoot();

        // Assert
        Assert.AreEqual(2, shootingComponent._weaponAmmo);
        Assert.IsTrue(bulletComponent.gameObject.activeSelf);
    }

    [Test]
    public void Reload_RefillsAmmoAndDecreasesClip()
    {
        // Arrange
        var reloadingComponent = new GameObject().AddComponent<WeaponProjectileManager>();
        reloadingComponent._weaponClip = 2;
        reloadingComponent._weaponAmmo = 10;
        reloadingComponent._weaponInfo = ScriptableObject.CreateInstance<WeaponInfo>();
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
