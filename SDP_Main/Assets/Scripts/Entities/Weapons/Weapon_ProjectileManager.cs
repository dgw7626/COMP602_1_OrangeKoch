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
    private Weapon_Info _weaponInfo;

    [SerializeField]
    internal List<Weapon_Bullet> _localBullets;

    [SerializeField]
    private int _weaponAmmo;

    [SerializeField]
    private int _weaponClip;

    [SerializeField]
    private AmmunitionUI _ammunitionUI;

    internal Coroutine _currentCoroutine;
    private Vector3 _fw,
        _up;
    private Transform _camera;
    public List<Transform> muzzleFlashObjects;
    public List<Transform> bulletTracerObjects;
    public List<Transform> bulletObjects;
    public List<Transform> shellObjects;
    public List<Transform> hitObjects;
    internal Transform _firePos;

    //for Synchronization
    public PhotonView photonView;
    internal string _playerNameID;
    public AudioMixerGroup masterMixer;
    void Awake()
    {
        if (!transform.GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        _playerNameID = PhotonNetwork.NickName;
        this._camera = this.transform.parent.GetComponentInChildren<Camera>().transform;
        GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        this._fw = _camera.transform.InverseTransformDirection(transform.forward);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._fw, nameof(this._fw));
        this._up = _camera.transform.InverseTransformDirection(transform.up);
        GuardClause.InspectGuardClauseNullRef<Vector3>(this._up, nameof(this._up));
    }

    void Start()
    {
        _ammunitionUI = transform.parent.GetComponentInChildren<AmmunitionUI>();
        _ammunitionUI.gameObject.SetActive(false);
        // Check if the view is mine
   
        photonView = GetComponent<PhotonView>();
        _ammunitionUI.gameObject.SetActive(true);
        
        _weaponAmmo = _weaponInfo.BulletCounts;
        _weaponClip = _weaponInfo.ClipCounts;
        _firePos = transform.GetChild(0).GetChild(0).transform;
        GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(
            this._ammunitionUI,
            nameof(this._ammunitionUI)
        );
        _ammunitionUI.SetAmmunition(_weaponAmmo, _weaponClip);
    }

    /// <summary>
    /// This method will update the target object of the postion and rotation, the position values will be duplicated from parent object.
    /// </summary>
    [PunRPC]
    public void UpdateChildTransform()
    {
        // GuardClause.InspectGuardClauseNullRef<Transform>(this._camera, nameof(this._camera));
        if (!transform.GetComponentInParent<PhotonView>().IsMine) return;
        var newFw = _camera.transform.TransformDirection(_fw);
        var newUp = _camera.transform.TransformDirection(_up);
        var newRot = Quaternion.LookRotation(newFw, newUp);
        this.transform.position = _camera.transform.position;
        this.transform.rotation = newRot;
        return;
    }

    [PunRPC]
    public void InitBullets_P()
    {
   

        Transform bulletsTransform = transform.Find("Bullets");
        if (bulletsTransform == null)
        {
            GameObject bullets = new GameObject("Bullets");
            bulletsTransform = bullets.transform;
            bulletsTransform.SetParent(transform);
        }

        Transform firePos = transform.GetChild(0).GetChild(0).transform;

        Debug.Log("called");

        for (int i = 0; i < _weaponInfo.BulletCounts; i++)
        {
            GameObject bulletObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Bullet"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            bulletObject.name = "(" + i + ")Bullet";
            bulletObject.GetComponent<AudioSource>().clip = _weaponInfo.ShootEffect;

            bulletObject.transform.SetParent(bulletsTransform);
            bulletObjects.Add(bulletObject.transform);

            GameObject muzzleFlash = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "MuzzleFlash"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            muzzleFlash.name = "(" + i + ")muzzleFlash";
            muzzleFlash.transform.SetParent(bulletObject.transform);
            muzzleFlash.SetActive(false);
            muzzleFlashObjects.Add(muzzleFlash.transform);

            GameObject bulletTrace = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "BulletTracer"), firePos.position + new Vector3(0, -3f, 0), _weaponInfo.BulletTrace.transform.rotation, 0);
            bulletTrace.name = "(" + i + ")bulletTracer";
            bulletTrace.transform.SetParent(bulletObject.transform);
            bulletTrace.SetActive(false);
            bulletTracerObjects.Add(bulletTrace.transform);

            GameObject shellObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Shell"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            shellObject.name = "(" + i + ")shell";
            shellObject.transform.SetParent(bulletObject.transform);
            shellObject.SetActive(false);
            shellObjects.Add(shellObject.transform);

            GameObject hitObject = PhotonNetwork.Instantiate(Path.Combine("LocalPrefabs", "Hit"), Vector3.zero + new Vector3(0, 0, 5), Quaternion.identity, 0);
            hitObject.transform.SetParent(bulletObject.transform);
            hitObject.name = "(" + i + ")hitObject";
            hitObject.GetComponent<AudioSource>().outputAudioMixerGroup = masterMixer;
            hitObject.SetActive(false);
            hitObjects.Add(hitObject.transform);

            bulletObject.SetActive(true);

            Weapon_Bullet weaponBullet = bulletObject.GetComponent<Weapon_Bullet>();
            if (weaponBullet != null)
            {
                weaponBullet._bulletIndex = i;
                _localBullets.Add(weaponBullet);
            }
        }
        //Kill the duplicates.
        transform.GetComponentInParent<PhotonView>().RPC(nameof(DestoryBulletVFXS),RpcTarget.All);
    }

    [PunRPC]
    public void DestoryBulletVFXS()
    {
        string[] targets = { "Bullet(Clone)", "MuzzleFlash(Clone)", "BulletTracer(Clone)", "Shell(Clone)", "Hit(Clone)" };
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // Get all objects in the scene

        List<GameObject> topLevelObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null) // Check if the object has no parent
            {
                topLevelObjects.Add(obj);
            }
        }

        // Print the names of the top-level objects
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
    ///  This method creates the bullet instance, it will create number of bullets based on WeaponInfo BulletCounts.
    /// </summary>
    [PunRPC]
    public void InitBullets()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        print("CALLED???");
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
        for (int i = 0; i < _weaponInfo.BulletCounts; i++)
        {
            var bulletObject = Instantiate(
                Resources.Load(Path.Combine("LocalPrefabs", "Bullet")) as GameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bullets.transform
            );
            
            bulletObject.name = "(" + i + ")Bullet";
            bulletObject.GetComponent<AudioSource>().clip = _weaponInfo.ShootEffect;
            bulletObjects.Add(bulletObject.transform);
          //  bulletObjects[i].gameObject.SetActive(true);

            var muzzleFlash = Instantiate(
                _weaponInfo.MuzzleFlash.gameObject,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            muzzleFlash.name = "(" + i + ")muzzleFlash";
            muzzleFlash.SetActive(false);
            muzzleFlashObjects.Add(muzzleFlash.transform);

            var bulletTrace = Instantiate(
                _weaponInfo.BulletTrace.gameObject,
                (firePos.position + new Vector3(0, -3f, 0)),
                _weaponInfo.BulletTrace.transform.rotation,
                bulletObject.transform
            );
            bulletTrace.name = "(" + i + ")bulletTracer";
            bulletTrace.SetActive(false);
            bulletTracerObjects.Add(bulletTrace.transform);

            var shellObject = Instantiate(
                _weaponInfo.BulletShell,
                Vector3.zero + new Vector3(0, 0, 5),
                Quaternion.identity,
                bulletObject.transform
            );
            shellObject.name = "(" + i + ")shell";
            shellObject.SetActive(false);
            shellObjects.Add(shellObject.transform);

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
            hitObject.name = "(" + i + ")hitObject";
            hitObject.SetActive(false);
            hitObjects.Add(hitObject.transform);
            //setting up the index number for the bullet
            bulletObject.GetComponent<Weapon_Bullet>()._bulletIndex = (int)i;
          //  Debug.Log("Bullet Index" + bulletObject.GetComponent<Weapon_Bullet>()._bulletIndex);
            //set the bullet object available.
            bulletObject.SetActive(true);
            
            _localBullets.Add(bulletObject.GetComponent<Weapon_Bullet>());
        }
        //getting all the vfx instances
        return;
    }

    /// <summary>
    /// This method is automatic shoot method.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    public IEnumerator GetShoot(float delaySecond)
    {
    //    var firePos = transform.GetChild(0).GetChild(0).transform;
      //  GuardClause.InspectGuardClauseNullRef<Transform>(_firePos, nameof(_firePos));
        foreach (Weapon_Bullet bullet in _localBullets)
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
    /// This method initiates Fire method to shoot, ammo will be dcreased by 1 after the shot.
    /// </summary>

    [PunRPC]
    public void GetShoot()
    {
     //   GuardClause.InspectGuardClauseNullRef<int>(_weaponAmmo, nameof(_weaponAmmo));
        if (_weaponAmmo >= 1)
        {
          //  var firePos = transform.GetChild(0).GetChild(0).transform;
            _weaponAmmo--;
            _ammunitionUI.UpdateUI(_weaponAmmo, _weaponClip);
            foreach (Transform bullet in bulletObjects)
            {
                //check the child object is not active.
                if (!bullet.transform.GetChild(0).gameObject.activeSelf)
                {
                    bullet.GetComponent<Weapon_Bullet>().Fire(_firePos);
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
       
      //  GuardClause.InspectGuardClauseNullRef<int>(this._weaponClip, nameof(this._weaponClip));
      //  GuardClause.InspectGuardClauseNullRef<int>(this._weaponAmmo, nameof(this._weaponAmmo));
      //  GuardClause.InspectGuardClauseNullRef<AmmunitionUI>(
      //      this._ammunitionUI,
      //      nameof(this._ammunitionUI)
      //  );
        if (this._weaponClip > 0)
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
                photonView.RPC(nameof(GetShoot),RpcTarget.All);
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
