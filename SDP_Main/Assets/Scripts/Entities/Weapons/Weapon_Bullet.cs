using Photon.Pun;
using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;


public class Weapon_Bullet : MonoBehaviourPun, IWeapon_Fireable
{
    internal GameObject _hit;
    internal GameObject _bulletTracer;
    internal GameObject _shell;
    internal GameObject _muzzleFlash;
    internal GameObject _bullet;
    internal Coroutine _currentCoroutine;
    public int _bulletIndex;
    internal int _currentIndex;
    internal Weapon_ProjectileManager _projectileManager;
    internal Weapon_Controller _projectController;
    private void Start()
    {
        //get the photon view instance.
        PhotonView photonView = transform.GetComponent<PhotonView>();
        //check if the photon view is not null and is current multiplayer.
        if (photonView != null && photonView.IsMine)
        {
            //get the current index of the transform
            _currentIndex = GetCurrentBuildIndex(transform.name);
            if (_bulletIndex == _currentIndex)
            {
                // if the current index meets get the Weapon Projecile manager class and return it.
                _projectileManager = transform.parent.parent.GetComponent<Weapon_ProjectileManager>();
                return;
            }
        }
        //get the player child object instance.
        Transform parentTransform = transform.parent;
        //check if the parent object is not null and check another parent object.
        if (parentTransform != null && parentTransform.parent != null)
        {
            //get the weapon controller class form parent object.
            Weapon_Controller controller = parentTransform.parent.GetComponent<Weapon_Controller>();
            //if the controller is not null and multiplayer is false.
            if (controller != null && !Game_RuntimeData.isMultiplayer)
            {
                // get the projectile manager class and controller class.
                _projectileManager = parentTransform.parent.GetComponent<Weapon_ProjectileManager>();
                _projectController = controller;
            }
        }
    }

    /// <summary>
    /// Author: Sky
    /// This method translates the transform name to numeraic value.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    internal int GetCurrentBuildIndex(string str)
    {
        // if the string is not null
        if (str != null)
        {
            //get the regex pattern
            string pattern = @"\b\d+\b";
            Regex regex = new Regex(pattern);
            //check if the regex matches
            MatchCollection matches = regex.Matches(str);
            //looping to check the matching interger values.
            foreach (Match match in matches)
            {
                int value = int.Parse(match.Value);
                return value;
            }
        }
        // if not found return 0.
        return 0;
    }
    /// <summary>
    /// Author: Sky
    /// This method implementes the shooting for multiplayer and local player.
    /// </summary>
    /// <param name="origin"></param>
    [PunRPC]
    public void Fire(Transform origin)
    {
        //get the current transform position of the origin.
        transform.position = origin.position;
        //insntatiating the bullet vfx instances.
        InstantiateGunVFX();
        //raycasting the origin position 
        //you need to chnange the origin position from here.
        if (Physics.Raycast(_projectileManager.transform.position, _projectileManager.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            //if the transform is not null.
            if (hit.transform != null)
            {
                //if the tag matches
                if (hit.transform.tag == "Player_Model")
                {
                    //it will call and check health and damage count.
                    HitPlayer(hit);
                }
                //check if the controller is not null and is multiplayer is false.
                if (_projectController != null && !Game_RuntimeData.isMultiplayer)
                {
                    //exectue the mutliplayer bullet trace vfx.
                    RenderGunTrace(hit.point, origin.position);
                    _currentCoroutine = StartCoroutine(DisableBullet(this._bullet.transform.GetComponent<AudioSource>().clip.length));
                    return;
                }
                //check if the photonnetwork is local. 
                if (PhotonNetwork.LocalPlayer == transform.GetComponent<PhotonView>().Owner)
                {
                    //exectue the mutliplayer bullet trace vfx.
                    transform.GetComponent<PhotonView>().RPC(nameof(RenderGunTrace), RpcTarget.AllBuffered, hit.point, origin.position);
                    _currentCoroutine = StartCoroutine(DisableBullet(this._bullet.transform.GetComponent<AudioSource>().clip.length));
                }
                return;
            }
        }

    }
    /// <summary>
    /// Author:Sky
    /// This method intantiates bullet vfx game instances.
    /// </summary>
    internal void InstantiateGunVFX()
    {
        //get all the object instances from the projectile manager class.
        this._shell = _projectileManager.shellObjects[_currentIndex].gameObject;
        this._bulletTracer = _projectileManager.bulletTracerObjects[_currentIndex].gameObject;
        this._muzzleFlash = _projectileManager.muzzleFlashObjects[_currentIndex].gameObject;
        this._hit = _projectileManager.hitObjects[_currentIndex].gameObject;
        this._bullet = _projectileManager.bulletObjects[_currentIndex].gameObject;
        //executing each vfx, shell is the rigidbody based vfx instance.
        this._shell.SetActive(true);
        this._shell = _projectileManager.shellObjects[_bulletIndex].gameObject;
        this._shell.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this._shell.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this._shell.transform.position = this.transform.parent.parent.GetChild(0).Find("BulletShellPos").position;
        //add a force to the shell game object in y position and x position.
        this._shell.GetComponent<Rigidbody>().AddForce((transform.up * Random.Range(160, 210)) + (transform.right * Random.Range(160, 210)));
        //enable the muzzle flash vfx.
        this._muzzleFlash.SetActive(true);
        //play the muzzle falsh particle system
        this._muzzleFlash.GetComponent<ParticleSystem>().Play();
        //play the audio source of the bullet instance.
        this._bullet.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Author: Sky
    /// This method renders the bullet trace. the paramteter takes hit and origin positions.
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="origin"></param>
    [PunRPC]
    internal void RenderGunTrace(Vector3 hit, Vector3 origin)
    {
        // Check if the PhotonView exists
        if (transform.GetComponent<PhotonView>() == null)
        {
            // Log an error or handle the missing PhotonView
            Debug.LogError("PhotonView not found!");
            return;
        }

        _hit.transform.position = hit;
        _hit.transform.parent = null;
        // bullet and hit set to active.
        this._bulletTracer.SetActive(true);
        this._hit.SetActive(true);
        //change the position to the origin position.
        this._bulletTracer.transform.position = origin;
        //get trailrenderer and add origin position.
        _bulletTracer.GetComponent<TrailRenderer>().AddPosition(origin);
        _bulletTracer.transform.position = hit;
        //set shell, bullet tracer parent's null so it dosent attach to the local player object.
        this._shell.transform.parent = null;
        _bulletTracer.transform.parent = null;
        // play the audio sound of the hit transform.
        _hit.transform.GetComponent<AudioSource>().Play();

        // Clean up the RPC call if the GameObject or PhotonView was destroyed
        if (PhotonNetwork.LocalPlayer == transform.GetComponent<PhotonView>().Owner)
        {
            // Clear the RPC buffer if the PhotonView was destroyed
            if (!PhotonNetwork.NetworkingClient.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.RemoveRPCs(transform.GetComponent<PhotonView>());
            }
        }
    }


    /// <summary>
    /// Author: Corey John Knight
    /// Creates a new damage struct, converts it to JSON. Gets the PV of the player that was hit and uses their
    /// PV to RPC call themselves to inform that they have been hit. They must then tell others that they were damaged.
    /// </summary>
    /// <param name="hit"></param>
    private void HitPlayer(RaycastHit hit)
    {
        if (!Game_RuntimeData.isMultiplayer)
        {
            return;
        }

        PhotonView pv = hit.transform.GetComponentInParent<Player_PlayerController>().photonView;
        if (pv == null)
        {
            Debug.LogError("WARNING: The player who was shot has no Photon View!");
            return;
        }

        s_DamageInfo dmg = new s_DamageInfo();
        dmg.bodyPart = e_BodyPart.NONE;
        dmg.dmgValue = 10f;
        dmg.dmgDealerId = Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber;
        dmg.dmgRecievedId = pv.Owner.ActorNumber;
        pv.RPC(nameof(Player_MultiplayerEntity.OnDamageRecieved), pv.Owner, JsonUtility.ToJson(dmg));
    }

    public void Hit(Transform origin)
    {
        //get the raycast from the origin position
        if (Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            //check if the transform is null.
            if (hit.transform != null)
            {
                //print the name of the other transform object.
                Debug.DrawLine(origin.position, hit.point, Color.yellow);
                Debug.unityLogger.logEnabled = true;
                Debug.Log(hit.transform.name + ": Hit");
                return;
            }
        }

    }
    /// <summary>
    /// Author: Sky
    /// The method waits for a specified delay, then performs various operations to disable certain game objects and stop a coroutine.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    internal IEnumerator DisableBullet(float delaySecond)
    {
        // waiting for the specific time of the delay second.
        yield return new WaitForSeconds(delaySecond);
        //change the _buleltTracer transform to the original bullet transform.
        _bulletTracer.transform.SetParent(transform);
        //change the rotatin to the camera postion.
        _bulletTracer.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation.eulerAngles.x + (-90f)), GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0);
        _hit.transform.SetParent(transform);
        this._shell.transform.SetParent(transform);
        //deactive all the object instances.
        this._shell.SetActive(false);
        this._bulletTracer.SetActive(false);
        this._muzzleFlash.SetActive(false);
        this._hit.SetActive(false);
        StopCoroutine(_currentCoroutine);
    }
}
