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
using Photon.Pun;
using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// this class is designed to send the damage to the player and responsible for making vfx effects to the bullet instance.
/// </summary>
public class Weapon_Bullet : MonoBehaviourPun, IWeapon_Fireable
{
    internal GameObject _hit;               //hit object so it can contain hit sound.
    internal GameObject _bulletTracer;      //bullet trace object.
    internal GameObject _shell;             //shell object that contains rigidbody component.
    internal GameObject _muzzleFlash;       //muzzle flash object that contains vfx instance.
    internal GameObject _bullet;            //bullet object that contains bullet script.
    internal Coroutine _currentCoroutine;   //custom coroutine to desroy the bullet instance.
    public int _bulletIndex;                //bullet index to check the current object of the index name.
    internal int _currentIndex;             //current index name for the current object name.
    internal Weapon_ProjectileManager _projectileManager;           //private weapon projectile manager class for the bullet object instance.
    internal Weapon_Controller _projectController;                  //private weapon controller class for the bullet object instance.
    public Player_MultiplayerEntity m_MultiplayerEntity;            //current mutliplayer entity class to get the current photon view component.
    public float WeaponDamage;              //current weapon damage value.
    public Transform FirePosition;          //current weapon fire position.
    internal bool _showGizmos = false;      //this is for checking if the bullet is present.
    /// <summary>
    ///  Functions to run once when object is initialized.
    /// </summary>
    private void Start()
    {
        //check if the photon view is not null and is current multiplayer.
        if (Game_RuntimeData.isMultiplayer && Game_RuntimeData.thisMachinesPlayersPhotonView.IsMine)
        {
                //get the current index of the transform
                _currentIndex = GetCurrentBuildIndex(transform.name);
                if (_bulletIndex == _currentIndex)
                {
                //if the current bullet object index matches to  the current object of the bullet index it will return with the projectile manager and multiplayer entitity.
                m_MultiplayerEntity = transform.parent.parent.parent.GetComponent<Player_MultiplayerEntity>();
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
    /// this method gets the current name of the object and translates to the integer value of the game object.
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
        // if it dosent match it will return 0 in default.
        return 0;
    }
    /// <summary>
    /// This method implementes the shooting for multiplayer and local player.
    /// </summary>
    /// <param name="origin"></param>
    public void Fire(Transform origin)
    {
        //check if its multiplayer gameobject instance and check if the photon view is mine.
        if (Game_RuntimeData.isMultiplayer && !Game_RuntimeData.thisMachinesPlayersPhotonView.IsMine)
            return;
        //get the current transform position of the origin.
        transform.position = origin.position;
        //insntatiating the bullet vfx instances.
        InstantiateGunVFX();
        //raycasting the origin position
        
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
                RenderGunTrace(hit.point, origin.position);
                _currentCoroutine = StartCoroutine(DisableBullet(this._bullet.transform.GetComponent<AudioSource>().clip.length));
                //check if the controller is not null and is multiplayer is false.
                if (_projectController != null && !Game_RuntimeData.isMultiplayer)
                {
                    //exectue the mutliplayer bullet trace vfx.
                    RenderGunTrace(hit.point, origin.position);
                    _currentCoroutine = StartCoroutine(DisableBullet(this._bullet.transform.GetComponent<AudioSource>().clip.length));
                    return;
                }
                //exectue the mutliplayer bullet trace vfx.
                return;
            }
        }

    }
    /// <summary>
    /// Debugging the shooting position and bullet position, so player can see which bullet its landing.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_showGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_projectileManager.transform.position, 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_hit.transform.position, 0.2f);
        }
    }
    /// <summary>
    /// This method intantiates bullet vfx game instances.
    /// </summary>
    public void InstantiateGunVFX()
    {
        //get all the object instances from the projectile manager class.
        this._shell = _projectileManager.ShellObjects[_currentIndex].gameObject;
        this._bulletTracer = _projectileManager.BulletTracerObjects[_currentIndex].gameObject;
        this._muzzleFlash = _projectileManager.MuzzleFlashObjects[_currentIndex].gameObject;
        this._hit = _projectileManager.HitObjects[_currentIndex].gameObject;
        this._bullet = _projectileManager.BulletObjects[_currentIndex].gameObject;
        //executing each vfx, shell is the rigidbody based vfx instance.
        this._shell.SetActive(true);
        this._shell = _projectileManager.ShellObjects[_bulletIndex].gameObject;
        //set the shell rigid body velocity to be zero.
        this._shell.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //set the shell rigid body angular velocity to be zero.
        this._shell.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this._shell.transform.position = this.transform.parent.parent.GetChild(0).Find("BulletShellPos").position;
        //add a force to shell postiton with random value numbers in transform y position and -x position with 160 ~ 210 value range.
        this._shell.GetComponent<Rigidbody>().AddForce((transform.up * Random.Range(160, 210)) + (transform.right * Random.Range(160, 210)));
        //enable the muzzle flash vfx.
        this._muzzleFlash.SetActive(true);
        //play the muzzle falsh particle system
        this._muzzleFlash.GetComponent<ParticleSystem>().Play();
        //play the audio source of the bullet instance.
        this._bullet.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// This method renders the bullet trace. the paramteter takes hit and origin positions.
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="origin"></param>

    internal void RenderGunTrace(Vector3 hit, Vector3 origin)
    {
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

    }


    /// <summary>
    /// Creates a new damage struct, converts it to JSON. Gets the PV of the player that was hit and uses their
    /// PV to RPC call themselves to inform that they have been hit. They must then tell others that they were damaged.
    /// </summary>
    /// <param name="hit"></param>
    private void HitPlayer(RaycastHit hit)
    {
        //check if the object instance is multiplayer instance.
        if (!Game_RuntimeData.isMultiplayer)
        {
            return;
        }
        //get the photon view component from the ray cast object.
        PhotonView pv = hit.transform.GetComponentInParent<Player_PlayerController>().photonView;
        //if the photon view object is null.
        if (pv == null)
        {
            Debug.LogError("WARNING: The player who was shot has no Photon View!");
            return;
        }
        //initalize the damange status struct and insert datas.
        s_DamageInfo dmg = new s_DamageInfo();
        dmg.bodyPart = e_BodyPart.NONE;
        //put damange value to be 10.
        dmg.dmgValue = 10f;
        //get the multiplayer instance actor number for example (1 or 2)
        dmg.dmgDealerId = m_MultiplayerEntity.playerController.photonView.Owner.ActorNumber;
        //get the photon view actor number (example 1, 2).
        dmg.dmgRecievedId = pv.Owner.ActorNumber;
        //get the ray cast object multiplayer instance team number.
        dmg.dmgRecievedTeam = hit.transform.GetComponentInParent<Player_MultiplayerEntity>().teamNumber;
        //get the current player gameobject instance team number.
        dmg.dmgDealerTeam = m_MultiplayerEntity.teamNumber;
        //call the pun rpc to send the damage status informatio to the OnDamageReceived. data to be json type.
        pv.RPC(nameof(Player_MultiplayerEntity.OnDamageRecieved), pv.Owner, JsonUtility.ToJson(dmg));
    }
    /// <summary>
    /// This method gets the target transform name with the debug.log messsage, once it shoot it shown in yellow line in z position.
    /// </summary>
    /// <param name="origin"></param>
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
                //debug log the transform object name with the hit message.
                Debug.Log(hit.transform.name + ": Hit");
                return;
            }
        }

    }
    /// <summary>
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
        //set the hit object parent to be bullet transform object.
        _hit.transform.SetParent(transform);
        //set the shell object parent to be bullet transform object.
        this._shell.transform.SetParent(transform);
        //deactive all the object instances.
        this._shell.SetActive(false);
        this._bulletTracer.SetActive(false);
        this._muzzleFlash.SetActive(false);
        this._hit.SetActive(false);
        //stop the coroutine one the method is ended.
        StopCoroutine(_currentCoroutine);
    }
}
