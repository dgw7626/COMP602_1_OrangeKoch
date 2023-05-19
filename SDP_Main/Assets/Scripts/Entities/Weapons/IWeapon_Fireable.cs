using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon_Fireable
{
    public void Fire(Transform origin);
    public void Hit(Transform origin);
 
}
