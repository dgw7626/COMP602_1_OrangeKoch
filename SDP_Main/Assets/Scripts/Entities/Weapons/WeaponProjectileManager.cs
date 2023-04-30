using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
public class WeaponProjectileManager : MonoBehaviour
{

    [SerializeField] WeaponInfo weaponInfo;
    [SerializeField] internal List<WeaponBullet> localBullets;
    internal Coroutine currentCoroutine;
    internal Vector3 pos, fw, up;
    [SerializeField] internal Transform fakeParent;
    [SerializeField] private int weaponAmmo;
    [SerializeField] private int weaponClip;
    [SerializeField] private AmmunitionUI ammo;
    void Start()
    {
        if (!transform.GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        weaponAmmo = weaponInfo.bulletCounts;
        weaponClip = weaponInfo.clipCounts;
        ammo = transform.parent.GetComponentInChildren<AmmunitionUI>();
       ammo.SetAmmunition(weaponAmmo, weaponClip);
        //    fakeParent = Camera.main.transform;
        pos = fakeParent.transform.InverseTransformPoint(transform.position);
        fw = fakeParent.transform.InverseTransformDirection(transform.forward);
        up = fakeParent.transform.InverseTransformDirection(transform.up);
    }
    public void UpdateChildTransform()
    {
        //checking if the fakeparent is not null and present.
        GuardClause.InspectGuardClause<Transform,Transform> (fakeParent.transform, ErrorTypes.NullRef);
        var newpos = fakeParent.transform.TransformPoint(pos);
        var newfw = fakeParent.transform.TransformDirection(fw);
        var newup = fakeParent.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;
    }

    public void InitBullets()
    {
        Transform bullets;
        if (transform.Find("Bullets") == null)
        {
            bullets = new GameObject("Bullets").transform;
            bullets.SetParent(transform);
        }
        bullets = transform.Find("Bullets");
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        for(int i = 0; i < weaponInfo.bulletCounts; i++)
        {
            GameObject bulletObject = Instantiate(Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject, Vector3.zero, Quaternion.identity,bullets.transform);
            bulletObject.name = "["+  i + "]" + bullets.name;
            bulletObject.GetComponent<AudioSource>().clip = weaponInfo.shootEffect;
            GameObject muzzleFlash = Instantiate(weaponInfo.muzzleFlash.gameObject, Vector3.zero, Quaternion.identity, bulletObject.transform);
            muzzleFlash.name = "[" + i + "]" + muzzleFlash.name;
            // Bullet Trace effects
            GameObject bulletTrace = Instantiate(weaponInfo.bulletTrace.gameObject, (firePos.position + new Vector3(0, -3f, 0)), weaponInfo.bulletTrace.transform.rotation, bulletObject.transform);
            bulletTrace.name = "[" + i + "]" + bulletTrace.name;
            GameObject hitObject = new GameObject();
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
            localBullets.Add(bulletObject.GetComponent<WeaponBullet>());
        }

    }
    public IEnumerator GetShoot(float delaySecond)
    {
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        foreach(WeaponBullet bullet in localBullets)
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
    public void GetShoot()
    {
        if (weaponAmmo >= 1)
        {
            Transform firePos = transform.GetChild(0).GetChild(0).transform;
            weaponAmmo--;
            ammo.UpdateUI(weaponAmmo, weaponClip);
            foreach (WeaponBullet bullet in localBullets)
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
    }
    public void InitShoot(WeaponFiretype fireType)
    {
        switch (fireType) {
            case WeaponFiretype.AUTO:
                {
                    currentCoroutine = StartCoroutine(GetShoot(0.1f));
                    break;
                }
            case WeaponFiretype.BURST:
                {

                    break;
                }
            case WeaponFiretype.SEMI:
                {
                    GetShoot();
                    break;
                }
            default:
                {
                    break;
                }
         }
    }
   


}
