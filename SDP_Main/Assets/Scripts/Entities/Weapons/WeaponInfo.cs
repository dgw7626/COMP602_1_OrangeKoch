using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaponData", menuName ="Weapons/WeaponData")]
public class WeaponInfo : ScriptableObject
{
   public string weaponName;
   public int bulletCounts;
   public int clipCounts;
   public ParticleSystem muzzleFlash;
    public ParticleSystem bulletTrace;
   public AudioClip shootEffect;
   public AudioClip reloadEffect;
    public AudioClip hitEffect;
}
