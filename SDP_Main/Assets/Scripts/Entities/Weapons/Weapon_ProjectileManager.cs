using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Weapon_ProjectileManager : MonoBehaviour
{

    [SerializeField] Weapon_WeaponInfo weaponInfo;
    [SerializeField] internal List<Weapon_Bullet> localBullets;
    internal Coroutine currentCoroutine;
    internal Vector3 pos, fw, up;
    [SerializeField] internal Transform fakeParent;
    void Start()
    {
    //    fakeParent = Camera.main.transform;
        pos = fakeParent.transform.InverseTransformPoint(transform.position);
        fw = fakeParent.transform.InverseTransformDirection(transform.forward);
        up = fakeParent.transform.InverseTransformDirection(transform.up);
    }
    public void UpdateChildTransform()
    {
        var newpos = fakeParent.transform.TransformPoint(pos);
        var newfw = fakeParent.transform.TransformDirection(fw);
        var newup = fakeParent.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;
    }
    
    public void InitBullets()
    {
        Transform bullets = transform.Find("Bullets");
        for(int i = 0; i < weaponInfo.bulletCounts; i++)
        {
            GameObject bulletObject = Instantiate(Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject, Vector3.zero, Quaternion.identity,bullets.transform);
            bulletObject.name = "["+  i + "]" + bullets.name;
            bulletObject.GetComponent<AudioSource>().clip = weaponInfo.shootEffect;
            GameObject muzzleFlash = Instantiate(weaponInfo.muzzleFlash.gameObject, Vector3.zero, Quaternion.identity, bulletObject.transform);
            muzzleFlash.name = "[" + i + "]" + muzzleFlash.name;
            GameObject hitObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //Ignore Raycast.
            hitObject.layer = 2;
            hitObject.AddComponent<AudioSource>().playOnAwake = false;
            hitObject.GetComponent<AudioSource>().clip = weaponInfo.hitEffect;
            hitObject.GetComponent<AudioSource>().spatialBlend = 1;
            hitObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            hitObject.GetComponent<AudioSource>().minDistance = 0;
            hitObject.GetComponent<AudioSource>().maxDistance = 20;
            hitObject.transform.SetParent(bulletObject.transform);
            //GameObject hitObject = Instantiate(gameObject , Vector3.zero, Quaternion.identity, bulletObject.transform);
            hitObject.name = "[" + i + "] hit Objects"; 

            bulletObject.SetActive(false);
            localBullets.Add(bulletObject.GetComponent<Weapon_Bullet>());
        }

    }
    public IEnumerator GetShoot(float delaySecond)
    {
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        foreach(Weapon_Bullet bullet in localBullets)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.gameObject.SetActive(true);
                bullet.Fire(firePos);

            }
            yield return new WaitForSeconds(delaySecond);
        //    bullet.gameObject.SetActive(false);
        }
        StopCoroutine(currentCoroutine);
    }
    public void GetShoot(bool isDebug)
    {
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        foreach (Weapon_Bullet bullet in localBullets)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.gameObject.SetActive(true);
                //   (isDebug  == true)? bullet.Hit(firePos) : bullet.Fire(firePos);
                bullet.Fire(firePos);
                return;
            }
            //bullet.gameObject.SetActive(false);
        }
    }
    public void InitShoot(Weapon_E_Firetype fireType)
    {
        switch (fireType) {
            case Weapon_E_Firetype.AUTO:
                {
                    currentCoroutine = StartCoroutine(GetShoot(0.1f));
                    break;
                }
            case Weapon_E_Firetype.BURST:
                {

                    break;
                }
            case Weapon_E_Firetype.SEMI:
                {
                    GetShoot(false);
                    break;
                }
            default:
                {
                    break;
                }
         }
    }
   


}
