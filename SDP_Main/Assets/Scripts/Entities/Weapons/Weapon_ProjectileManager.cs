using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using Photon.Pun;
//[RequireComponent(typeof(PhotonView))]
public class Weapon_ProjectileManager : MonoBehaviour
{
    [Header("Weapon Controls")]
    [SerializeField]
    public Weapon_Info WeaponInfo;

    [SerializeField]
    public List<Weapon_Bullet> LocalBullets;

    [SerializeField]
    public int WeaponAmmo;

    [SerializeField]
    public int WeaponClip;

    [SerializeField]
    public AmmunitionUI AmmunitionUI;
    private Weapon_Controller _weaponController;
    internal Coroutine _currentCoroutine;
    private Vector3 _fw, _up;
    private Transform _camera;
    [HideInInspector]
    public List<Transform> MuzzleFlashObjects;
    [HideInInspector]
    public List<Transform> BulletTracerObjects;
    [HideInInspector]
    public List<Transform> BulletObjects;
    [HideInInspector]
    public List<Transform> ShellObjects;
    [HideInInspector]
    public List<Transform> HitObjects;
    internal Transform _firePos;

    public PhotonView PhotonView;
    public AudioMixerGroup MasterMixer;
    void Awake()
    {
        //check if the photon is mine.
        if (!transform.GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        //get the camera rotation and position by calling transform methods.
        this._camera = this.transform.parent.GetComponentInChildren<Camera>().transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        this._fw = _camera.transform.InverseTransformDirection(transform.forward);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._fw, nameof(this._fw));
        this._up = _camera.transform.InverseTransformDirection(transform.up);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._up, nameof(this._up));
    }

    void Start()
    {
        //get all the ammunition and weapon controller classes.
        AmmunitionUI = transform.parent.GetComponentInChildren<AmmunitionUI>();
        _weaponController = transform.GetComponent<Weapon_Controller>();
        AmmunitionUI.gameObject.SetActive(false);

        //if the transform object is current multiplayer.
        if (Game_RuntimeData.isMultiplayer && transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            AmmunitionUI.gameObject.SetActive(true);
        } else if(!Game_RuntimeData.isMultiplayer)
        {
            AmmunitionUI.gameObject.SetActive(true);
        }

        PhotonView = GetComponent<PhotonView>();
        WeaponAmmo = WeaponInfo.BulletCounts;
        WeaponClip = WeaponInfo.ClipCounts;
        _firePos = transform.GetChild(0).GetChild(0).transform;
        GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(
            this.AmmunitionUI,
            nameof(this.AmmunitionUI)
        );
        //update the ammunition ammos.
        AmmunitionUI.SetAmmunition(WeaponAmmo, WeaponClip);
    }

    /// <summary>
    /// Author: Sky
    /// This method will update the target object of the postion and rotation, the position values will be duplicated from parent object.
    /// </summary>
    [PunRPC]
    public void UpdateChildTransform()
    {
        //update camera object rotation and position.
        if (!transform.GetComponentInParent<PhotonView>().IsMine) return;
        var newFw = _camera.transform.TransformDirection(_fw);
        var newUp = _camera.transform.TransformDirection(_up);
        var newRot = Quaternion.LookRotation(newFw, newUp);
        this.transform.position = _camera.transform.position;
        this.transform.rotation = newRot;
        return;
    }


    /// <summary>
    /// Auther: Sky
    /// method is responsible for destroying specific game objects related to bullet visual effects in a game scene.
    /// </summary>
    [PunRPC]
    public void InitBullets_P()
    {
        //check if the bullet object instance is not null.
        Transform bulletsTransform = transform.Find("Bullets");
        if (bulletsTransform == null)
        {
            //create bullets object instance if its null.
            GameObject bullets = new GameObject("Bullets");
            bulletsTransform = bullets.transform;
            bulletsTransform.SetParent(transform);
        }
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        for (int i = 0; i < WeaponInfo.BulletCounts; i++)
        {
            //instantating bullet object throught the photon network.
            GameObject bulletObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Bullet"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            bulletObject.name = "(" + i + ")Bullet";
            bulletObject.GetComponent<AudioSource>().clip = WeaponInfo.ShootEffect;

            bulletObject.transform.SetParent(bulletsTransform);
            BulletObjects.Add(bulletObject.transform);
            //instantating muzzleflash object throught the photon network.
            GameObject muzzleFlash = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "MuzzleFlash"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            muzzleFlash.name = "(" + i + ")muzzleFlash";
            muzzleFlash.transform.SetParent(bulletObject.transform);
            muzzleFlash.SetActive(false);
            MuzzleFlashObjects.Add(muzzleFlash.transform);

            //instantating bullet tracer object throught the photon network.
            GameObject bulletTrace = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "BulletTracer"), firePos.position + new Vector3(0, -3f, 0), WeaponInfo.BulletTrace.transform.rotation, 0);
            bulletTrace.name = "(" + i + ")bulletTracer";
            bulletTrace.transform.SetParent(bulletObject.transform);
            bulletTrace.SetActive(false);
            BulletTracerObjects.Add(bulletTrace.transform);

            //instantating shell object throught the photon network.
            GameObject shellObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Shell"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            shellObject.name = "(" + i + ")shell";
            shellObject.transform.SetParent(bulletObject.transform);
            shellObject.SetActive(false);
            ShellObjects.Add(shellObject.transform);

            //instantating hit object throught the photon network.
            GameObject hitObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Hit"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            hitObject.transform.SetParent(bulletObject.transform);
            hitObject.name = "(" + i + ")hitObject";
            hitObject.GetComponent<AudioSource>().outputAudioMixerGroup = MasterMixer;
            hitObject.SetActive(false);
            HitObjects.Add(hitObject.transform);

            bulletObject.SetActive(true);

            Weapon_Bullet weaponBullet = bulletObject.GetComponent<Weapon_Bullet>();
            if (weaponBullet != null)
            {
                //set the bullet index to the iterator.
                weaponBullet.BulletIndex = i;
                LocalBullets.Add(weaponBullet);
            }
        }
        //destory all the remaining instantation (there is a bug that creates antoher bullet intance).
        transform.GetComponentInParent<PhotonView>().RPC(nameof(DestoryBulletVFXS),RpcTarget.All);
    }

    /// <summary>
    /// Auther: Sky
    /// This method is responsible for destroying specific game objects related to bullet visual effects in a game scene.
    /// </summary>
    [PunRPC]
    public void DestoryBulletVFXS()
    {
        string[] targets = { "Bullet(Clone)", "MuzzleFlash(Clone)", "BulletTracer(Clone)", "Shell(Clone)", "Hit(Clone)" };
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        List<GameObject> topLevelObjects = new List<GameObject>();

        //get all the parent objects.
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null) 
            {
                topLevelObjects.Add(obj);
            }
        }
        //iterating thorugh all the parent objects and destory them.
        foreach (GameObject topLevelObj in topLevelObjects)
        {
            for(int i = 0; i < targets.Length; i++)
            {
                if (targets[i].Contains(topLevelObj.transform.name))
                {
                    Destroy(topLevelObj.gameObject);
                }
            }
        }
        Debug.Log("cleared");
    }

    /// <summary>
    ///  Author: Sky
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts.
    /// </summary>
    public void InitBullets()
    { 
        Transform bullets;
        //check if the bullet object instnace is null
        if (transform.Find("Bullets") == null)
        {
            //create if the bullet object instnace is null.
            bullets = new GameObject("Bullets").transform;
            bullets.SetParent(transform);
        }
        bullets = transform.Find("Bullets");
        GuardClause.InspectGuardClauseNullRef<Transform>(bullets, nameof(bullets));
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(firePos, nameof(firePos));
        //iterate through all weapon bullet counts.
        for (int i = 0; i < WeaponInfo.BulletCounts; i++)
        {
            //create bullet object instnace in bulelts transform position.
            var bulletObject = Instantiate(
                Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bullets.transform
            );


            //create muzzle flash object instnace in bulelts transform position.
            bulletObject.name = "(" + i + ")Bullet";
            bulletObject.GetComponent<AudioSource>().clip = WeaponInfo.ShootEffect;
            BulletObjects.Add(bulletObject.transform);
            var muzzleFlash = Instantiate(
                WeaponInfo.MuzzleFlash.gameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            muzzleFlash.name = "(" + i + ")muzzleFlash";
            muzzleFlash.SetActive(false);
            MuzzleFlashObjects.Add(muzzleFlash.transform);


            //create bullet tracer object instnace in bulelts transform position.
            var bulletTrace = Instantiate(
                WeaponInfo.BulletTrace.gameObject,
                (firePos.position + new Vector3(0, -3f, 0)),
                WeaponInfo.BulletTrace.transform.rotation,
                bulletObject.transform
            );
            bulletTrace.name = "(" + i + ")bulletTracer";
            bulletTrace.SetActive(false);
            BulletTracerObjects.Add(bulletTrace.transform);

            //create shell object instnace in bulelts transform position.
            var shellObject = Instantiate(
                WeaponInfo.BulletShell,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            shellObject.name = "(" + i + ")shell";
            shellObject.SetActive(false);
            ShellObjects.Add(shellObject.transform);


            //create hit object instnace in bulelts transform position.
            var hitObject = new GameObject();
            hitObject.transform.position = new Vector3(0, 0, 5);
            hitObject.layer = 2;
            hitObject.AddComponent<AudioSource>().playOnAwake = false;
            hitObject.GetComponent<AudioSource>().clip = WeaponInfo.HitEffect;
            hitObject.GetComponent<AudioSource>().spatialBlend = 1;
            hitObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            hitObject.GetComponent<AudioSource>().minDistance = 0;
            hitObject.GetComponent<AudioSource>().maxDistance = 20;
            hitObject.transform.SetParent(bulletObject.transform);
            hitObject.name = "(" + i + ")hitObject";
            hitObject.SetActive(false);
            HitObjects.Add(hitObject.transform);
            bulletObject.GetComponent<Weapon_Bullet>().BulletIndex = (int)i;
            bulletObject.SetActive(true);
            
            //add the local bullet object instance to the weapon_bullet class.
            LocalBullets.Add(bulletObject.GetComponent<Weapon_Bullet>());
        }
        return;
    }

    /// <summary>
    /// Author: Sky
    /// This method is automatic shoot method.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    public IEnumerator GetShoot(float delaySecond)
    {
        foreach (Weapon_Bullet bullet in LocalBullets)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.Fire(_firePos);
                bullet.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(delaySecond);
        }
        StopCoroutine(_currentCoroutine);
    }

    /// <summary>
    /// Author: Sky
    /// This method initiates Fire method to shoot, ammo will be dcreased by 1 after the shot.
    /// </summary>
    [PunRPC]
    public void GetShoot()
    {
        if (WeaponAmmo >= 1)
        {
            WeaponAmmo--;
            //update the current ammo.
            AmmunitionUI.UpdateUI(WeaponAmmo, WeaponClip);
            foreach (Transform bullet in BulletObjects)
            {
                if (!bullet.transform.GetChild(0).gameObject.activeSelf)
                {
                    bullet.GetComponent<Weapon_Bullet>().Fire(_firePos);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Author: Sky
    ///  This method reloads the current gun, ammo display will be refreshed after reload.
    /// </summary>
    [PunRPC]
    public void Reload()
    {
        //only reloads when the clip is greater than 0.
        if (this.WeaponClip > 0)
        {
            //update the current ammo.
            this.WeaponAmmo = WeaponInfo.BulletCounts;
            this.WeaponClip--;
            AmmunitionUI.SetAmmunition(this.WeaponAmmo, this.WeaponClip);
        }
    }

    /// <summary>
    /// Author: Sky
    /// This method initialize the shooting type of the weapon there are three options (Auto, Burst, Semi).
    /// </summary>
    /// <param name="fireType"></param>
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
                    //get rpc shoot if the mutliplayer is enabled other wise it calls local method.
                    if (Game_RuntimeData.isMultiplayer)
                    {
                        PhotonView.RPC(nameof(GetShoot), RpcTarget.AllBuffered);
                        break;
                    }
                    else
                    {
                        GetShoot();
                        break;
                    }
            }
            default:
            {
                break;
            }
        }
        return;
    }
}
