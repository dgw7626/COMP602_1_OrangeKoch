using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;

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
        PhotonView photonView = transform.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine)
        {
            _currentIndex = GetCurrentBuildIndex(transform.name);
            if (_bulletIndex == _currentIndex)
            {
                _projectileManager = transform.parent.parent.GetComponent<Weapon_ProjectileManager>();
                return;
            }
        }

        Transform parentTransform = transform.parent;
        if (parentTransform != null && parentTransform.parent != null)
        {
            Weapon_Controller controller = parentTransform.parent.GetComponent<Weapon_Controller>();
            if (controller != null && !controller.isMultiplayer)
            {
                _projectileManager = parentTransform.parent.GetComponent<Weapon_ProjectileManager>();
                _projectController = controller;
            }
        }
    }

    /// <summary>
    /// This method translates the transform name to numeraic value.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    internal int GetCurrentBuildIndex(string str)
    {
        if (str != null)
        { 
            string pattern = @"\b\d+\b";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(str);
            foreach (Match match in matches)
            {
                int value = int.Parse(match.Value);
                return value;
            }
        }
       
        return 0;
    }
    /// <summary>
    /// This method implementes the shooting for multiplayer and local player.
    /// </summary>
    /// <param name="origin"></param>
    [PunRPC]
    public void Fire(Transform origin)
    {
        transform.position = origin.position;
        InstantiateGunVFX();
            if (Physics.Raycast(origin.position, _projectileManager.transform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Player_Model")
                    {
                        HitPlayer(hit);
                    }
                if (_projectController != null && !_projectController.isMultiplayer)
                {
                    RenderGunTrace(hit.point, origin.position);
                    _currentCoroutine = StartCoroutine(DisableBullet(this._bullet.transform.GetComponent<AudioSource>().clip.length));
                    return;
                }
                if(PhotonNetwork.LocalPlayer == transform.GetComponent<PhotonView>().Owner) { 
                transform.GetComponent<PhotonView>().RPC(nameof(RenderGunTrace), RpcTarget.AllBuffered,hit.point, origin.position);
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
        this._shell = _projectileManager.shellObjects[_currentIndex].gameObject;
        this._bulletTracer = _projectileManager.bulletTracerObjects[_currentIndex].gameObject;
        this._muzzleFlash = _projectileManager.muzzleFlashObjects[_currentIndex].gameObject;
        this._hit = _projectileManager.hitObjects[_currentIndex].gameObject;
        this._bullet = _projectileManager.bulletObjects[_currentIndex].gameObject;
        this._shell.SetActive(true);
        this._shell = _projectileManager.shellObjects[_bulletIndex].gameObject;
        this._shell.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this._shell.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this._shell.transform.position = this.transform.parent.parent.GetChild(0).Find("BulletShellPos").position;
        this._shell.GetComponent<Rigidbody>().AddForce((transform.up * Random.Range(160, 210)) + (transform.right * Random.Range(160, 210)));
        this._muzzleFlash.SetActive(true);
        this._muzzleFlash.GetComponent<ParticleSystem>().Play();
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
        this._bulletTracer.SetActive(true);
        this._hit.SetActive(true);
        this._bulletTracer.transform.position = origin;
        _bulletTracer.GetComponent<TrailRenderer>().AddPosition(origin);
        _bulletTracer.transform.position = hit;
        this._shell.transform.parent = null;
        _bulletTracer.transform.parent = null;
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
        PhotonView pv = hit.transform.GetComponentInParent<Player_PlayerController>().photonView;
        if(pv == null)
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
        GuardClause.InspectGuardClauseNullRef<Transform>(origin, nameof(origin));
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
    /// <summary>
    /// Author: Sky
    /// The method waits for a specified delay, then performs various operations to disable certain game objects and stop a coroutine.
    /// </summary>
    /// <param name="delaySecond"></param>
    /// <returns></returns>
    internal IEnumerator DisableBullet( float delaySecond)
    {
        yield return new WaitForSeconds(delaySecond);
        _bulletTracer.transform.SetParent(transform);
        _bulletTracer.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation.eulerAngles.x + (-90f)), GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0);
        _hit.transform.SetParent(transform);
        this._shell.transform.SetParent(transform);
        this._shell.SetActive(false);
        this._bulletTracer.SetActive(false);
        this._muzzleFlash.SetActive(false);
        this._hit.SetActive(false);
        StopCoroutine(_currentCoroutine);
    }
}
