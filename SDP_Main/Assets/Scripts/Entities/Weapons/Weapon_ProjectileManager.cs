/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Hanul Rheem		            *
 * Student ID: 		20109218		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using Photon.Pun;
/// <summary>
/// this class is designed to control the weapon controller and bullet instance and managing weapon information.
/// </summary>
public class Weapon_ProjectileManager : MonoBehaviour
{
    [Header("Weapon Controls")]
    public Weapon_Info WeaponInfo;              //Scriptable object data that contains weapon information.
    public List<Weapon_Bullet> LocalBullets;    //Weapon bullet script lists
    public int WeaponAmmo;                      //Weapon ammo
    public int WeaponClip;                      //Weapon megazine.
    public AmmunitionUI AmmunitionUI;           //Ammunition UI class for the setting up the weapon ui display.
    private Weapon_Controller _weaponController;//Private weapon controller script for weapon projectile manager.
    internal Coroutine _currentCoroutine;       //Private coroutine for the  destorying reaminaing bullet object instances.
    private Vector3 _fw, _up;                   //private vector3 for forward and up transform position.
    private Transform _camera;                  //private transform camera position.
    public List<Transform> MuzzleFlashObjects;  //Muzzle flash vfx lists
    public List<Transform> BulletTracerObjects; //Bullet trace vfx lists
    public List<Transform> BulletObjects;       //bullet script lists
    public List<Transform> ShellObjects;        //shell rigidbody lists
    public List<Transform> HitObjects;          //hit transform object lists.
    internal Transform _firePos;                //current position of the gun fire position.
    public PhotonView PhotonView;               //current photon view for this class
    public AudioMixerGroup MasterMixer;         //current audio group for this class
    public bool isIntialized = false; 
    /// <summary>
    ///  Functions to run once when object is instantiated.
    /// </summary>
    void Awake()
    {
        //check if the photon is mine.
        if (!Game_RuntimeData.isMultiplayer && !transform.GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        //get the camera rotation and position by calling transform methods.
        this._camera = this.transform.parent.GetComponentInChildren<Camera>().transform.Find("Player_Raycast").transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        this._fw = _camera.transform.InverseTransformDirection(transform.forward);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._fw, nameof(this._fw));
        this._up = _camera.transform.InverseTransformDirection(transform.up);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._up, nameof(this._up));
    }

    /// <summary>
    ///  Functions to run once when object is initialized.
    /// </summary>
    void Start()
    {
        //get all the ammunition and weapon controller classes.
        AmmunitionUI = transform.parent.GetComponentInChildren<AmmunitionUI>();
        _weaponController = transform.GetComponent<Weapon_Controller>();
        AmmunitionUI.gameObject.SetActive(false);

        //if the player is multiplayer, it will enable the ammunition ui display.
        if (Game_RuntimeData.isMultiplayer && transform.parent.GetComponent<Player_PlayerController>().photonView.IsMine)
        {
            AmmunitionUI.gameObject.SetActive(true);
        } 
        //if the player is local multiplayer, it will enable the ammunition ui display.
        else if(!Game_RuntimeData.isMultiplayer)
        {
            AmmunitionUI.gameObject.SetActive(true);
        }
        //getting the current photon view object instace.
        PhotonView = GetComponent<PhotonView>();
        //getting the current weapon ammo
        WeaponAmmo = WeaponInfo.BulletCounts;
        //getting the current weapon megazine.
        WeaponClip = WeaponInfo.ClipCounts;
        //getting the currrent gun fire transform position.
        _firePos = transform.GetChild(0).GetChild(0).transform;
        //check if the object type is present or matches to the type casting.
        GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(
            this.AmmunitionUI,
            nameof(this.AmmunitionUI)
        );
        //update the ammunition ammos.
        AmmunitionUI.SetAmmunition(WeaponAmmo, WeaponClip);
    }

    /// <summary>
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
    /// method is responsible for destroying specific game objects related to bullet visual effects in a game scene.
    /// </summary>
    [PunRPC]
    public void RPC_InitBullets_P()
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
            bulletObject.GetComponent<Weapon_Bullet>().m_MultiplayerEntity = transform.parent.GetComponent<Player_MultiplayerEntity>();
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
    /// This method is responsible for destroying specific game objects related to bullet visual effects in a game scene.
    /// </summary>
    [PunRPC]
    public void DestoryBulletVFXS()
    {
        //get the taraget object instance names.
        string[] targets = { "Bullet(Clone)", "MuzzleFlash(Clone)", "BulletTracer(Clone)", "Shell(Clone)", "Hit(Clone)" };
        //get all the present active/deactive game objects.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        //get the list of the top level objects.
        List<GameObject> topLevelObjects = new List<GameObject>();

        //get all the parent objects.
        foreach (GameObject obj in allObjects)
        {
            //if the parent is null, it will be defined as hierachy object.
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
                    //destory the remaining game objects.
                    Destroy(topLevelObj.gameObject);
                }
            }
        }
    }

    /// <summary>
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts. Manual mode.
    /// </summary>
    public void ManualInitBullets(ParticleSystem muzzleFlashPrefab, TrailRenderer bulletTracePrefab, GameObject bulletShellPrefab, AudioClip shootEffectPrefab, AudioClip hitEffectPrefab)
    {
        //make a local variable object for the transform.
        Transform bullets;
        //check if the bullet object instnace is null
        if (transform.Find("Bullets") == null)
        {
            //create if the bullet object instnace is null.
            bullets = new GameObject("Bullets").transform;
            bullets.SetParent(transform);
        }
        bullets = transform.Find("Bullets");
        //check if the buellet object is matched to the type.
        GuardClause.InspectGuardClauseNullRef<Transform>(bullets, nameof(bullets));
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        //check if the firepos has the transsform componen type.
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
            bulletObject.GetComponent<AudioSource>().clip = shootEffectPrefab;
            BulletObjects.Add(bulletObject.transform);
            var muzzleFlash = Instantiate(
                muzzleFlashPrefab.gameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            muzzleFlash.name = "(" + i + ")muzzleFlash";
            muzzleFlash.SetActive(false);
            MuzzleFlashObjects.Add(muzzleFlash.transform);


            //create bullet tracer object instnace in bulelts transform position.
            var bulletTrace = Instantiate(
                bulletTracePrefab.gameObject,
                (firePos.position + new Vector3(0, -3f, 0)),
                WeaponInfo.BulletTrace.transform.rotation,
                bulletObject.transform
            );
            bulletTrace.name = "(" + i + ")bulletTracer";
            bulletTrace.SetActive(false);
            BulletTracerObjects.Add(bulletTrace.transform);

            //create shell object instnace in bulelts transform position.
            var shellObject = Instantiate(
                bulletShellPrefab,
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
            //add audio source component to the hit object and deactivate the playOnAwake.
            hitObject.AddComponent<AudioSource>().playOnAwake = false;
            //change the clip to be hit effect sounnd
            hitObject.GetComponent<AudioSource>().clip = hitEffectPrefab;
            //change the 3d sound settings to be 1.
            hitObject.GetComponent<AudioSource>().spatialBlend = 1;
            //change the rolloffmode to be linear sound, so the sound is linear to hear.
            hitObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            //change the minimum distance to be 0.
            hitObject.GetComponent<AudioSource>().minDistance = 0;
            //change the maximum distance to be 20.
            hitObject.GetComponent<AudioSource>().maxDistance = 20;
            //set hit transform parent to be bullet script object transform.
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
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts.
    /// </summary>
    public void InitBullets()
    {
        //make a local variable object for the transform.
        Transform bullets;
        //check if the bullet object instnace is null
        if (transform.Find("Bullets") == null)
        {
            //create if the bullet object instnace is null.
            bullets = new GameObject("Bullets").transform;
            bullets.SetParent(transform);
        }
        bullets = transform.Find("Bullets");
        //check if the buellet object is matched to the type.
        GuardClause.InspectGuardClauseNullRef<Transform>(bullets, nameof(bullets));
        Transform firePos = transform.GetChild(0).GetChild(0).transform;
        //check if the firepos has the transsform componen type.
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
            //add audio source component to the hit object and deactivate the playOnAwake.
            hitObject.AddComponent<AudioSource>().playOnAwake = false;
            //change the clip to be hit effect sounnd
            hitObject.GetComponent<AudioSource>().clip = WeaponInfo.HitEffect;
            //change the 3d sound settings to be 1.
            hitObject.GetComponent<AudioSource>().spatialBlend = 1;
            //change the rolloffmode to be linear sound, so the sound is linear to hear.
            hitObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
            //change the minimum distance to be 0.
            hitObject.GetComponent<AudioSource>().minDistance = 0;
            //change the maximum distance to be 20.
            hitObject.GetComponent<AudioSource>().maxDistance = 20;
            //set hit transform parent to be bullet script object transform.
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
    /// This method is automatic shoot method.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    public IEnumerator GetShoot(float delaySecond)
    {
        //iterate through all the bullet script object instances.
        foreach (Weapon_Bullet bullet in LocalBullets)
        {
            //if the bullet object is not active.
            if (!bullet.gameObject.activeSelf)
            {
                //call bullet fire method with the gun fire position.
                bullet.Fire(_firePos);
                //deactviate the current bullet object instance.
                bullet.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(delaySecond);
        }
        //stop the coriiutine once its called by this method.
        StopCoroutine(_currentCoroutine);
    }

    /// <summary>
    /// This method initiates Fire method to shoot, ammo will be dcreased by 1 after the shot.
    /// </summary>
    [PunRPC]
    public void GetShoot()
    {
        //if the weapon ammo is greater than the 1.
        if (WeaponAmmo >= 1)
        {
            //decrease the bullet ammo by 1.
            WeaponAmmo--;
            //update the current ammo.
            AmmunitionUI.UpdateUI(WeaponAmmo, WeaponClip);
            //interate through all the bullet game object instances.
            foreach (Transform bullet in BulletObjects)
            {
                //if the bullet object instance is deactive.
                if (!bullet.transform.GetChild(0).gameObject.activeSelf)
                {
                    //get the bullet class object 
                    Weapon_Bullet weaponBullet = bullet.GetComponent<Weapon_Bullet>();
                    //call bullet fire method with the gun fire position.
                    weaponBullet.Fire(_firePos);
                    return;
                }
            }
        }
    }

    /// <summary>
    ///  This method reloads the current gun, ammo display will be refreshed after reload.
    /// </summary>
    [PunRPC]
    public void RPC_Reload()
    {
        //only reloads when the clip is greater than 0.
        if (this.WeaponClip > 0)
        {
            //update the current ammo.
            this.WeaponAmmo = WeaponInfo.BulletCounts;
            //weapon clip will be decreased by 1.
            this.WeaponClip--;
            AmmunitionUI.SetAmmunition(this.WeaponAmmo, this.WeaponClip);
        }
    }
    /// <summary>
    /// This method reloads the current gun, ammo display will be refreshed after reload.
    /// </summary>
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
    /// This method initialize the shooting type of the weapon there are three options (Auto, Burst, Semi).
    /// </summary>
    /// <param name="fireType"></param>
    public void InitShoot(Weapon_Firetype fireType)
    {
        //check the fire type.
        switch (fireType)
        {
            //if its auto the bullet will shoot automatically.
            case Weapon_Firetype.Auto:
            {
                _currentCoroutine = StartCoroutine(GetShoot(0.1f));
                break;
            }
            case Weapon_Firetype.Burst:
            {
                break;
            }
            //if its semi the bullet will shoot each time.
            case Weapon_Firetype.Semi:
            {
                    //get rpc shoot if the mutliplayer is enabled other wise it calls local method.
                    if (Game_RuntimeData.isMultiplayer)
                    {
                        transform.GetComponent<PhotonView>().RPC(nameof(GetShoot),RpcTarget.All);
                        break;
                    }
                    //if the player is local multiplayer, shoots in local method.
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
    /// <summary>
    /// Reset the Ammo when player respawn
    /// </summary>
    public void ResetAmmo()
    {
        WeaponAmmo = WeaponInfo.BulletCounts;
        WeaponClip = WeaponInfo.ClipCounts;

        AmmunitionUI.SetAmmunition(WeaponAmmo, WeaponClip);
    }
}
