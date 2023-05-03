using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
public class WeaponProjectileManager : MonoBehaviour
{

    [SerializeField] private WeaponInfo _weaponInfo;
    [SerializeField] internal List<WeaponBullet> _localBullets;
    [SerializeField] private int _weaponAmmo;
    [SerializeField] private int _weaponClip;
    [SerializeField] private AmmunitionUI _ammunitionUI;

    internal Coroutine _currentCoroutine;
    private Vector3 _fw, _up;
    private Transform _camera;
 
    void Awake()
    {
        if (!transform.GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        this._camera = this.transform.parent.GetComponentInChildren<Camera>().transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        this._fw = _camera.transform.InverseTransformDirection(transform.forward);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._fw, nameof(this._fw));
        this._up = _camera.transform.InverseTransformDirection(transform.up);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._up, nameof(this._up));
    }
    void Start()
    {
        _weaponAmmo = _weaponInfo.BulletCounts;
        _weaponClip = _weaponInfo.ClipCounts;
        _ammunitionUI = transform.parent.GetComponentInChildren<AmmunitionUI>();
        GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(this._ammunitionUI, nameof(this._ammunitionUI));
        _ammunitionUI.SetAmmunition(_weaponAmmo, _weaponClip);
    }
    /// <summary>
    /// This method will update the target object of the postion and rotation, the position values will be duplicated from parent object.
    /// </summary>
    public void UpdateChildTransform()
    {
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        var newFw = _camera.transform.TransformDirection(_fw);
        var newUp = _camera.transform.TransformDirection(_up);
        var newRot = Quaternion.LookRotation(newFw, newUp);
        this.transform.position = _camera.transform.position;
        this.transform.rotation = newRot;
        return;
    }
    /// <summary>
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts.
    /// </summary>
    public void InitBullets()
    {
        Transform bullets;
        if (transform.Find("Bullets") == null)
        {
            bullets = new GameObject("Bullets").transform;
            bullets.SetParent(transform);
        }
        bullets = transform.Find("Bullets");
        GuardClause.InspectGuardClauseNullRef<Transform>(bullets, nameof(bullets));
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(firePos, nameof(firePos));
        for(int i = 0; i < _weaponInfo.BulletCounts; i++)
        {
            var bulletObject = Instantiate(Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject, Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity,bullets.transform);
            bulletObject.name = "("+  i + ")" + bullets.name;
            bulletObject.GetComponent<AudioSource>().clip = _weaponInfo.ShootEffect;
            var muzzleFlash = Instantiate(_weaponInfo.MuzzleFlash.gameObject, Vector3.zero + new Vector3(0,0,5), Quaternion.identity, bulletObject.transform);
            muzzleFlash.name = "(" + i + ")" + muzzleFlash.name;
            var bulletTrace = Instantiate(_weaponInfo.BulletTrace.gameObject, (firePos.position + new Vector3(0, -3f, 0)), _weaponInfo.BulletTrace.transform.rotation, bulletObject.transform);
            bulletTrace.name = "(" + i + ")" + bulletTrace.name;
            var shellObject = Instantiate(_weaponInfo.BulletShell, Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, bulletObject.transform);
            shellObject.name = "(" + i + ")" + shellObject.name;
            var hitObject = new GameObject();
            hitObject.transform.position = new Vector3(0, 0, 5);
            hitObject.layer = 2;
            hitObject.AddComponent<AudioSource>().playOnAwake = false;
            hitObject.GetComponent<AudioSource>().clip = _weaponInfo.HitEffect;
            hitObject.GetComponent<AudioSource>().spatialBlend = 1;
            hitObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            hitObject.GetComponent<AudioSource>().minDistance = 0;
            hitObject.GetComponent<AudioSource>().maxDistance = 20;
            hitObject.transform.SetParent(bulletObject.transform);
            hitObject.name = "(" + i + ")hitObjects";
          
            bulletObject.SetActive(false);
            _localBullets.Add(bulletObject.GetComponent<WeaponBullet>());
        }
        return;
    }
    /// <summary>
    /// This method is automatic shoot method.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    public IEnumerator GetShoot(float delaySecond)
    {
        var firePos = transform.GetChild(0).GetChild(0).transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(firePos, nameof(firePos));
        foreach(WeaponBullet bullet in _localBullets)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.Fire(firePos);
                bullet.gameObject.SetActive(true);

            }
            yield return new WaitForSeconds(delaySecond);
        }
        StopCoroutine(_currentCoroutine);
    }
    /// <summary>
    /// This method initiates Fire method to shoot, ammo will be dcreased by 1 after the shot.
    /// </summary>
    public void GetShoot()
    {
        GuardClause.InspectGuardClauseNullRef<int>(_weaponAmmo, nameof(_weaponAmmo));
        if (_weaponAmmo >= 1)
        {
            var firePos = transform.GetChild(0).GetChild(0).transform;
            _weaponAmmo--;
            _ammunitionUI.UpdateUI(_weaponAmmo, _weaponClip);
            foreach (WeaponBullet bullet in _localBullets)
            {
                if (!bullet.gameObject.activeSelf)
                {
                    bullet.gameObject.SetActive(true);
                    bullet.Fire(firePos);
                    return;
                }
            }
        }
    }
    /// <summary>
    ///  This method reloads the current gun, ammo display will be refreshed after reload.
    /// </summary>
    public void Reload()
    {
        GuardClause.InspectGuardClauseNullRef<int>(this._weaponClip, nameof(this._weaponClip));
        GuardClause.InspectGuardClauseNullRef<int>(this._weaponAmmo, nameof(this._weaponAmmo));
        GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(this._ammunitionUI, nameof(this._ammunitionUI));
        if(this._weaponClip > 0)
        {
            this._weaponAmmo = _weaponInfo.BulletCounts;
            this._weaponClip--;
            _ammunitionUI.SetAmmunition(this._weaponAmmo, this._weaponClip);
        }
    }
    /// <summary>
    /// This method initialize the shooting type of the weapon there are three options (Auto, Burst, Semi).
    /// </summary>
    /// <param name="fireType"></param>
    public void InitShoot(WeaponFiretype fireType)
    {
        switch (fireType) {
            case WeaponFiretype.Auto:
                {
                    _currentCoroutine = StartCoroutine(GetShoot(0.1f));
                    break;
                }
            case WeaponFiretype.Burst:
                {

                    break;
                }
            case WeaponFiretype.Semi:
                {
                    GetShoot();
                    break;
                }
            default:
                {
                    break;
                }
         }
        return;
    }
   


}
