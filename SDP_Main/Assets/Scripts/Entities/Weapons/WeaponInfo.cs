using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaponData", menuName ="Weapons/WeaponData")]
public class WeaponInfo : ScriptableObject
{
   
   public string WeaponName;
   public int BulletCounts;
   public int ClipCounts;
   public ParticleSystem MuzzleFlash;
   public TrailRenderer BulletTrace;
    public GameObject BulletShell;
   public AudioClip ShootEffect;
   public AudioClip ReloadEffect;
   public AudioClip HitEffect;
}
