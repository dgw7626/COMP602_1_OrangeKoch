using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bullet : MonoBehaviour, Weapon_I_Fireable
{
    // need a direction to determin the hit point of the surface.
    public void Fire(Transform origin)
    {
        if(Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit,Mathf.Infinity))
        {
            if(hit.transform != null)
            {
                Debug.DrawLine(origin.position,hit.point, Color.red);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Fire");
                return;
            }           
        }
    }

    public void Hit(Transform origin)
    {
        if (Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform != null)
            {
                Debug.DrawRay(origin.position, hit.point, Color.yellow);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Hit");
                return;
            }
        }

    }
}
