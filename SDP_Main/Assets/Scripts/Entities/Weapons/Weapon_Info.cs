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
// Creating the asset data for the weaponData the menu will be Weapons/WeaponData.
[CreateAssetMenu(fileName ="WeaponData", menuName ="Weapons/WeaponData")]
///<summary>
/// This is weapon information class that contains storeable object like JSON/XML this is scritpalbe object for serializable object.
/// </summary>
public class Weapon_Info : ScriptableObject
{
   public string WeaponName;            //Weapon object type name
   public int BulletCounts;             //Weapon ammos
   public int ClipCounts;               //Weapon megazines
   public ParticleSystem MuzzleFlash;   //Weapon muzzle flash vfx
   public TrailRenderer BulletTrace;    //Weapon bullet trace vfx
    public GameObject BulletShell;      //Weapon bullet shell vfx
   public AudioClip ShootEffect;        //Weapon shoot sfx
   public AudioClip ReloadEffect;       //Weapon reload sfx
   public AudioClip HitEffect;          //Object hit sfx
}
