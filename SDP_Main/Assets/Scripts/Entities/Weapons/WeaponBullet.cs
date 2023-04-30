using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour, IWeaponFireable
{
    // need a direction to determin the hit point of the surface.
    internal GameObject _hitObject;
    internal GameObject _bulletObject;
    internal Coroutine _currentCoroutine;
    
    public void Fire(Transform origin)
    {
        transform.position = origin.position;
        Transform vfx = transform.GetChild(0).transform;
        Transform bulletVfx = transform.GetChild(1).transform;
        Transform hitVfx = transform.GetChild(2).transform;
        bulletVfx.transform.position = transform.position;
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
                hitVfx.parent = null;
                bulletVfx.parent = null;
                _bulletObject = bulletVfx.gameObject;
                _hitObject = hitVfx.gameObject;
                hitVfx.GetComponent<AudioSource>().Play();
                _currentCoroutine = StartCoroutine(DisableBullet(transform.GetComponent<AudioSource>().clip.length));
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
        Transform tempPos = _bulletObject.transform;
        _bulletObject.transform.SetParent(transform);
        _bulletObject.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation.eulerAngles.x + (-90f)), GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0);
        _hitObject.transform.SetParent(transform);
        transform.gameObject.SetActive(false);
        StopCoroutine(_currentCoroutine);
    }
}
