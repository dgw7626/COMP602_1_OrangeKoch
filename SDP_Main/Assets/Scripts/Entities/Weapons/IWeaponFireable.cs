using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponFireable
{
    public void Fire(Transform origin);
    public void Hit(Transform origin);
 
}
