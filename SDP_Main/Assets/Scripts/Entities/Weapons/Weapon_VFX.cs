using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_VFX : MonoBehaviour
{
    internal Transform _muzzleFlashObjects;
    internal Transform _bulletTracerObjects;
    internal Transform _bulletObjects;
    internal Transform _shellObjects;
    internal Transform _hitObjects;
    internal int _index;
    public Weapon_VFX() { }
    public Weapon_VFX(Transform muzzle, Transform bulletTracer, Transform bullet, Transform shell,Transform hit,int index)
    {
        this._muzzleFlashObjects = muzzle;
        this._bulletTracerObjects = bulletTracer;   
        this._bulletObjects = bullet;
        this._shellObjects = shell;
        this._hitObjects = hit;
        this._index = index;
    }

}
