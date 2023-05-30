using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.IO;

public class Weapon_System_Tests : MonoBehaviour
{
    internal Weapon_Bullet weapon_Bullet;
    [SetUp]
    public void Bullet_Init()
    {
        var bulletObject = Instantiate(
              Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject,
              Vector3.zero,
              Quaternion.identity);
        this.weapon_Bullet = bulletObject.GetComponent<Weapon_Bullet>();    
        if(bulletObject != null)
        {
            Debug.Log("Weapon Bullet Script Instantiated");
        }
    }
    [Test]
    public void Test_SetDamageValueTo10()
    {
        float finalValue = 10.0f;
        weapon_Bullet.weaponDamage = 10.0f;
        Assert.AreEqual(weapon_Bullet.weaponDamage, finalValue);
        return;
    }
    [Test]
    public void Test_SetBulletIndexTo5()
    {
        int finalValue = 5;
        weapon_Bullet._bulletIndex = 5;
        Assert.AreEqual(weapon_Bullet._bulletIndex, finalValue);
        return;
    }
    [Test]
    public void Test_CheckVectorVFX()
    {
        
        
        
    }
}
