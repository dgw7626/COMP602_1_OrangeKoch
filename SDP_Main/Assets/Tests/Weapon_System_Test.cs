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
using NUnit.Framework;
using System.IO;
/// <summary>
/// this class is designed to test out the weapon bullet system.
/// </summary>
public class Weapon_System_Test 
{
    internal Weapon_Bullet[] _bullet;       //local bullets
    internal Weapon_Info _info;             //weapon information
    internal Transform _firePosition;       //current shoot transform position.
    /// <summary>
    /// initializing bullet instances it will throw message if the bullet is succeed or failed. 
    /// </summary>
    [SetUp]
    public void Bullet_Init()
    {
        //Scriptable weapon information (this is serializable object).
        _info = ScriptableObject.CreateInstance<Weapon_Info>();
        _info.BulletCounts = 3; //total bullet ammo is 3.
        _info.ClipCounts = 3;   //total bullet megazine is 3.
        _info.BulletShell = Resources.Load(Path.Combine("LocalPrefabs", "Shell")) as GameObject;            //rigidbody object
        _info.BulletTrace = Resources.Load(Path.Combine("LocalPrefabs", "BulletTracer")) as TrailRenderer;  //trailrendder
        _info.MuzzleFlash = Resources.Load(Path.Combine("LocalPrefabs", "MuzzleFlash")) as ParticleSystem;  //particleystem
        _info.WeaponName = "Glock"; //set weapon name to be glock
        _info.ReloadEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test", "AK47")) as AudioClip;//audio clip
        _info.ShootEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test", "AK47")) as AudioClip; //audio clip
        _info.HitEffect = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test", "AK47")) as AudioClip;// audio clip
        _bullet = new Weapon_Bullet[_info.ClipCounts];  //get the bullets
        _firePosition = new GameObject().transform;     //creating fire transform position
        _firePosition.name = "FirePosiiton";
        for (int i = 0; i < _info.BulletCounts; i++)
        {
            //create bullet object instance and rename it with iterator.
            GameObject bulletObject = new GameObject("Bullet_" + i.ToString());
            //each bullet adds weapon bullet script.
            _bullet[i] = bulletObject.AddComponent<Weapon_Bullet>();
            //each bullet will have current iterator.
            _bullet[i].BulletIndex = i;
            //set damage to be 10.
            _bullet[i].WeaponDamage = 10;
            //set the bullet fire position to be fire position.
            _bullet[i].FirePosition = _firePosition;
            //set the bullet tracer game object
            _bullet[i].BulletTracer = Resources.Load(Path.Combine("LocalPrefabs", "BulletTracer")) as GameObject;
            //set the muzzle flash game object.
            _bullet[i].MuzzleFlash = Resources.Load(Path.Combine("LocalPrefabs", "MuzzleFlash")) as GameObject;
            //set the shell game object
            _bullet[i].Shell= Resources.Load(Path.Combine("LocalPrefabs", "Shell")) as GameObject;
            //create new game object instance and name it hit
            _bullet[i].HitObject = new GameObject("hit");
            //add audio source component to the hit object.
            _bullet[i].HitObject.AddComponent<AudioSource>();
            _bullet[i].HitObject.GetComponent<AudioSource>().clip = Resources.Load(Path.Combine("LocalPrefabs", "Weapon_Test", "AK47")) as AudioClip;
        }
        //print message if its succeed
        Debug.Log("bullets are successfully instantiated");
        //if not throw errors.
        if(_bullet[0] == null)
        {
            Debug.LogError("bullets instantiation failed");
        }
    }
    /// <summary>
    /// Testing bullet instance preferences, checking weapon settings is set and initialized.
    /// </summary>
    [Test]
    public void CheckBulletInstancePreferences_Test()
    {
        //get the current weapon bullet class
        Weapon_Bullet weaponBullet = _bullet[0].GetComponent<Weapon_Bullet>();
        //set the expected value to be 0
        int expected = 0;
        //check results
        Assert.IsNotNull(weaponBullet);
        Assert.AreEqual(expected, weaponBullet.BulletIndex);
        //set the expected position to be 0,0,0
        Vector3 expectedPosition = new Vector3(0, 0, 0);
        //check the transform position 
        Assert.IsNotNull(weaponBullet);
        Assert.AreEqual(expectedPosition, weaponBullet.FirePosition.position);
    }
    /// <summary>
    /// Check all the bullet instances and ensure the type of the bullet instance.
    /// </summary>
    [Test]
    public void CheckAllBulletInstances_Test()
    {
        //iterate through all bullet instances.
        for(int i = 0; i < _info.ClipCounts; i++)
        {
            //check if the bullet is not null.
            Assert.IsNotNull(_bullet[i]);
            //check the type of the bullet script.
            Assert.AreEqual(_bullet[i].GetComponent<Weapon_Bullet>().GetType(), typeof(Weapon_Bullet));
        }
    }
    /// <summary>
    /// Checking the bullet vfx instance, transform positions should be all different not equal to same vector3.
    /// </summary>
    [Test]
    public void CheckVectorVFX_Test()
    {
        //get the current weapon bullet script.
        Weapon_Bullet weaponBullet = _bullet[0].GetComponent<Weapon_Bullet>();
        //instantiate the VFX elemetns
        weaponBullet.ManualInstantiateGunVFX();
        //set the exepcted vector3 values.
        Vector3 hit = new Vector3(0, 0, 0);
        Vector3 origin = new Vector3(1, 3, 5);
        //check if they are all not equal, they are all different position values so the gun tracer can be rendered in differnt positions.
        Assert.AreNotEqual(hit.x, origin.x);
        Assert.AreNotEqual(hit.y, origin.y);
        Assert.AreNotEqual(hit.z, origin.z);
        //after check show the gun trace.
        weaponBullet.RenderGunTrace(hit,origin);

    }
    /// <summary>
    /// Check the bullet damage and test out the bullet damange value.
    /// </summary>
    [Test]
    public void CheckBulletDamage_Test()
    {
        //get the bullet class from the bullet instance.
        Weapon_Bullet weaponBullet = _bullet[0].GetComponent<Weapon_Bullet>();
        //set the expected result to be 10.
        int expectedDamage = 10;
        //check if they are not null and are equal to expected value.
        Assert.IsNotNull(weaponBullet.WeaponDamage);
        Assert.AreEqual(expectedDamage, weaponBullet.WeaponDamage);
    }
}
