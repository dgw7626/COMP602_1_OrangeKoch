using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bullet : MonoBehaviour, Weapon_I_Fireable
{
    // need a direction to determin the hit point of the surface.
    
    public void Fire(Transform origin)
    {
        transform.position = origin.position;
        Transform vfx = transform.GetChild(0).transform;
        Transform hitVfx = transform.GetChild(1).transform;
        vfx.GetComponent<ParticleSystem>().Play();
        transform.GetComponent<AudioSource>().Play();
        if (Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit,Mathf.Infinity))
        {
            if(hit.transform != null)
            {
                Debug.DrawLine(origin.position,hit.point, Color.red);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Fire");
                hitVfx.position = hit.point;
                hitVfx.GetComponent<AudioSource>().Play();
                Invoke(nameof(DisableBullet),transform.GetComponent<AudioSource>().clip.length);
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
                Debug.DrawLine(origin.position, hit.point, Color.yellow);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Hit");
                return;
            }
        }

    }

    internal void DisableBullet()
    {
        transform.gameObject.SetActive(false);
        return;
    }
}
