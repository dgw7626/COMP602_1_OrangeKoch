using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bullet : MonoBehaviour, Weapon_I_Fireable
{
    // need a direction to determin the hit point of the surface.
    internal GameObject hitObject;
    internal GameObject bulletObject;
    internal Coroutine currentCoroutine;

    public void Fire(Transform origin)
    {
        transform.position = origin.position;
        Transform vfx = transform.GetChild(0).transform;
        Transform bulletVfx = transform.GetChild(1).transform;
        Transform hitVfx = transform.GetChild(2).transform;
        bulletVfx.transform.position = transform.position;
     //   print(GetComponentInParent<Player_InputManager>().transform.eulerAngles.y + " |" + GetComponentInParent<Player_InputManager>().transform.rotation.y);
       // bulletVfx.transform.rotation = new Quaternion(0, GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0, 0);
       // bulletVfx.transform.rotation = Quaternion.AngLe
        //(origin.rotation.eulerAngles.x + (-90f))
     //   bulletVfx.transform.rotation = Quaternion.AngleAxis((Camera.main.transform.rotation.eulerAngles.x + (-90f)), Vector3.right);
      //  print(Quaternion.AngleAxis((Camera.main.transform.rotation.eulerAngles.x + (-90f)), Vector3.right));
        vfx.GetComponent<ParticleSystem>().Play();
        bulletVfx.GetComponent<ParticleSystem>().Play();
        transform.GetComponent<AudioSource>().Play();
        if (Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit,Mathf.Infinity))
        {
            if(hit.transform != null)
            {
                Debug.DrawLine(origin.position,hit.point, Color.red);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Fire");
                hitVfx.position = hit.point;
                //testing hit location
                hitVfx.parent = null;
                bulletVfx.parent = null;
                bulletObject = bulletVfx.gameObject;
                hitObject = hitVfx.gameObject;
                hitVfx.GetComponent<AudioSource>().Play();
                currentCoroutine = StartCoroutine(DisableBullet(transform.GetComponent<AudioSource>().clip.length));
               // Invoke(nameof(DisableBullet),transform.GetComponent<AudioSource>().clip.length);
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

    internal IEnumerator DisableBullet(float delaySecond)
    {
        yield return new WaitForSeconds(delaySecond);
        Transform tempPos = bulletObject.transform;
       // print(tempPos.rotation);// -0.7 is the 90 degree
       // print(tempPos.eulerAngles);
        bulletObject.transform.SetParent(transform);
        bulletObject.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation.eulerAngles.x + (-90f)), GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0);
        //bulletObject.transform.position = transform.position;
        //bulletObject.transform.rotation = new Quaternion((transform.rotation.x + (-0.7f)), transform.rotation.y, transform.rotation.z, 0);
        // bulletObject.transform.position = transform.position;
        hitObject.transform.SetParent(transform);
        transform.gameObject.SetActive(false);
        StopCoroutine(currentCoroutine);
    }
}
