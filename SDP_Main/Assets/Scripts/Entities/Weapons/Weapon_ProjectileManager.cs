using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using Newtonsoft.Json.Serialization;

<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
public class WeaponProjectileManager : MonoBehaviourPun
{
=======
public class Weapon_ProjectileManager : MonoBehaviour
{
    [SerializeField]
    private Weapon_Info _weaponInfo;

    [SerializeField]
    internal List<Weapon_Bullet> _localBullets;

    [SerializeField]
    private int _weaponAmmo;

    [SerializeField]
    private int _weaponClip;

    [SerializeField]
    private AmmunitionUI _ammunitionUI;
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs

    [SerializeField] private WeaponInfo _weaponInfo;
    [SerializeField] internal List<WeaponBullet> _localBullets;
    [SerializeField] private int _weaponAmmo;
    [SerializeField] private int _weaponClip;
    [SerializeField] private AmmunitionUI _ammunitionUI;
    [SerializeField] private WeaponController _weaponController;
    internal Coroutine _currentCoroutine;
    private Vector3 _fw, _up;
    private Transform _camera;
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
 
=======

    public List<Transform> muzzleVfxs;
    public List<Transform> bulletVfxs;
    public List<Transform> shellObjects;
    public List<Transform> hitObjects;

    internal void GetObjectVfxs()
    {
        Transform bullets = transform.Find("Bullets");
        GuardClause.InspectGuardClauseNullRef<Transform>(bullets, nameof(bullets));
        foreach (Transform bullet in bullets)
        {
            //getting MuzzleFlash VFX instances
            muzzleVfxs.Add(bullet.GetChild(0));
            //getting BuleltTracer VFX instnaces
            bulletVfxs.Add(bullet.GetChild(1));
            //getting Shell object instances
            shellObjects.Add(bullet.GetChild(2));
            //getting Hit object instances
            hitObjects.Add(bullet.GetChild(3));
        }
      //  return;
        
    }

>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
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

        _weaponController = GetComponent<WeaponController>();

    }
    /// <summary>
    /// This method will update the target object of the postion and rotation, the position values will be duplicated from parent object.
    /// </summary>
    [PunRPC]
    public void UpdateChildTransform()
    {
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        var newFw = _camera.transform.TransformDirection(_fw);
        var newUp = _camera.transform.TransformDirection(_up);
        var newRot = Quaternion.LookRotation(newFw, newUp);
        this.transform.position = _camera.transform.position;
        this.transform.rotation = newRot;
    }
    /// <summary>
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts.
    /// </summary>
    [PunRPC]
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
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
            var bulletObject = Instantiate(Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject, Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity,bullets.transform);
            bulletObject.name = "("+  i + ")" + bullets.name;
            bulletObject.GetComponent<AudioSource>().clip = _weaponInfo.ShootEffect;
            var muzzleFlash = Instantiate(_weaponInfo.MuzzleFlash.gameObject, Vector3.zero + new Vector3(0,0,5), Quaternion.identity, bulletObject.transform);
            muzzleFlash.name = "(" + i + ")" + muzzleFlash.name;
            var bulletTrace = Instantiate(_weaponInfo.BulletTrace.gameObject, (firePos.position + new Vector3(0, -3f, 0)), _weaponInfo.BulletTrace.transform.rotation, bulletObject.transform);
            bulletTrace.name = "(" + i + ")" + bulletTrace.name;
            var shellObject = Instantiate(_weaponInfo.BulletShell, Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, bulletObject.transform);
            shellObject.name = "(" + i + ")" + shellObject.name;
=======
            var bulletObject = Instantiate(
                Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bullets.transform
            );
            bulletObject.name = "(" + i + ")Bullet";
            bulletObject.GetComponent<AudioSource>().clip = _weaponInfo.ShootEffect;
            var muzzleFlash = Instantiate(
                _weaponInfo.MuzzleFlash.gameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            muzzleFlash.name = "(" + i + ")MuzzleFlash";
            var bulletTrace = Instantiate(
                _weaponInfo.BulletTrace.gameObject,
                (firePos.position + new Vector3(0, -3f, 0)),
                _weaponInfo.BulletTrace.transform.rotation,
                bulletObject.transform
            );
            bulletTrace.name = "(" + i + ")BulletTracer";
            var shellObject = Instantiate(
                _weaponInfo.BulletShell,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            shellObject.name = "(" + i + ")Shell";
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
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
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
            hitObject.name = "(" + i + ")hitObjects";
          
=======
            hitObject.name = "(" + i + ")HitObject";

>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
            bulletObject.SetActive(false);
            _localBullets.Add(bulletObject.GetComponent<Weapon_Bullet>());
        }
        GetObjectVfxs();
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
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
        foreach(WeaponBullet bullet in _localBullets)
=======
        foreach (Weapon_Bullet bullet in _localBullets)
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
        {
            if (!bullet.gameObject.activeSelf)
            {
                GetComponent<PhotonView>().RPC(nameof(bullet.Fire),RpcTarget.All, firePos);
                bullet.gameObject.SetActive(true);

            }
            yield return new WaitForSeconds(delaySecond);
        }
        StopCoroutine(_currentCoroutine);
    }
    /// <summary>
    /// This method initiates Fire method to shoot, ammo will be dcreased by 1 after the shot.
    /// </summary>
    [PunRPC]
    public void GetShoot()
    {
        GuardClause.InspectGuardClauseNullRef<int>(_weaponAmmo, nameof(_weaponAmmo));
        if (_weaponAmmo >= 1)
        {
            var firePos = transform.GetChild(0).GetChild(0).transform;
            print(firePos);

            _weaponAmmo--;
            _ammunitionUI.UpdateUI(_weaponAmmo, _weaponClip);
            foreach (Weapon_Bullet bullet in _localBullets)
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
    [PunRPC]
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
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
    [PunRPC]
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
                    GetComponent<PhotonView>().RPC("GetShoot", RpcTarget.All);
                  //  transform.GetComponentInParent<PhotonView>().RPC(nameof(GetShoot), RpcTarget.All);
                    //GetShoot();
                    break;
                }
=======
    public void InitShoot(Weapon_Firetype fireType)
    {
        switch (fireType)
        {
            case Weapon_Firetype.Auto:
            {
                _currentCoroutine = StartCoroutine(GetShoot(0.1f));
                break;
            }
            case Weapon_Firetype.Burst:
            {
                break;
            }
            case Weapon_Firetype.Semi:
            {
                GetShoot();
                break;
            }
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
            default:
                {
                    break;
                }
         }
        return;
    }
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponProjectileManager.cs
   


}
=======
}
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_ProjectileManager.cs
