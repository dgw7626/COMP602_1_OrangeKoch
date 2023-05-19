using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponBullet.cs
using Photon.Pun;
=======
using static UnityEngine.UI.Image;
using System.Text.RegularExpressions;
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Bullet.cs

public class Weapon_Bullet : MonoBehaviour, IWeapon_Fireable
{
    internal GameObject _hitObject;
    internal GameObject _bulletObject;
    internal GameObject _shellObject;
    internal Coroutine _currentCoroutine;
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponBullet.cs
    [PunRPC]
=======
    internal Weapon_ProjectileManager _projectileManager;
    internal int _bulletIndex;

    private void Start()
    {
        _projectileManager = transform.parent.parent.GetComponent<Weapon_ProjectileManager>();
        _bulletIndex = GetIntegerValueFromString(transform.name);
        print(_projectileManager.muzzleVfxs[_bulletIndex].name);
    }

    internal int GetIntegerValueFromString(string str)
    {
        string pattern = @"\b\d+\b";
        Regex regex = new Regex(pattern);
        MatchCollection matches =regex.Matches(str);

        foreach (Match match in matches)
        {
            int value = int.Parse(match.Value);
            return value;
        }
        return 0;
    }
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Bullet.cs
    public void Fire(Transform origin)
    {
        //VFX. 
        GuardClause.InspectGuardClauseNullRef<Transform>(origin, nameof(origin));
        transform.position = origin.position;
        //get the index and compare with the value.
       
        var muzzleVfx = transform.Find(transform.ToString().Substring(0,3)+ "MuzzleFlash").transform;
        var bulletVfx = transform.Find(transform.ToString().Substring(0, 3) + "BulletTracer").transform;
        this._bulletObject = bulletVfx.gameObject;
        GuardClause.InspectGuardClauseNullRef<GameObject>(this._bulletObject, nameof(this._bulletObject));
        this._bulletObject.transform.position = origin.position;
        var hitVfx = transform.Find(transform.ToString().Substring(0, 3) + "hitObjects").transform;
        this._hitObject = hitVfx.gameObject;
        GuardClause.InspectGuardClauseNullRef<GameObject>(this._hitObject, nameof(this._hitObject));
        var shellObj = transform.Find(transform.ToString().Substring(0, 3) + "Shell").transform;
        this._shellObject = shellObj.gameObject;
        this._shellObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this._shellObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this._shellObject.transform.position = this.transform.parent.parent.GetChild(0).Find("BulletShellPos").position;
        this._shellObject.GetComponent<Rigidbody>().AddForce((transform.up * Random.Range(80,100)) + (transform.right * Random.Range(80, 100)));
        muzzleVfx.GetComponent<ParticleSystem>().Play();
        transform.GetComponent<AudioSource>().Play();
        if (Physics.Raycast(origin.position, Camera.main.transform.forward, out RaycastHit hit,Mathf.Infinity))
        {
            if(hit.transform != null)
            {
                // Edit by Corey Knight - 04/05/23
                //--------------------------------
                if(hit.transform.tag == "Player_Model")
                {
                    Player_MultiplayerEntity e = hit.transform.GetComponentInParent<Player_MultiplayerEntity>();

                    Game_RuntimeData.thisMachinesMultiplayerEntity.DamagePlayer(e.playerController.photonView.Owner.ActorNumber);
                   /* Debug.Log("A Player was hit by " + Game_RuntimeData.thisMachinesPlayersPhotonView.ViewID + ". " +
                        "\nThe player that was hit was: " + e.uniqueID);*/
                }
                //--------------------------------
                Debug.DrawLine(origin.position,hit.point, Color.red);
                Debug.unityLogger.logEnabled = true;
                Debug.Log("Tagged Object : " + hit.transform.name);
                hitVfx.position = hit.point;
                hitVfx.parent = null;
                bulletVfx.GetComponent<TrailRenderer>().AddPosition(origin.position);
                bulletVfx.position = hit.point;
                this._shellObject.transform.parent = null; 
                bulletVfx.parent = null;
                hitVfx.GetComponent<AudioSource>().Play();
                _currentCoroutine = StartCoroutine(DisableBullet(transform.GetComponent<AudioSource>().clip.length));
                return;
            }           
        }
    }

<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponBullet.cs
=======
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
    /// <summary>
    /// This method is for 
    /// </summary>
    /// <param name="origin"></param>
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Bullet.cs
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
<<<<<<< Updated upstream:SDP_Main/Assets/Scripts/Entities/Weapons/WeaponBullet.cs
    
=======
   /// <summary>
   /// This method disables the current bullet object of instance.
   /// </summary>
   /// <param name="delaySecond"></param>
   /// <returns></returns>
>>>>>>> Stashed changes:SDP_Main/Assets/Scripts/Entities/Weapons/Weapon_Bullet.cs
    internal IEnumerator DisableBullet( float delaySecond)
    {
        GuardClause.InspectGuardClauseNullRef<GameObject>(this._bulletObject, nameof(this._bulletObject));
        yield return new WaitForSeconds(delaySecond);
        _bulletObject.transform.SetParent(transform);
        _bulletObject.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation.eulerAngles.x + (-90f)), GetComponentInParent<Player_InputManager>().transform.rotation.eulerAngles.y, 0);
        _hitObject.transform.SetParent(transform);
        this._shellObject.transform.SetParent(transform);
        transform.gameObject.SetActive(false);
        StopCoroutine(_currentCoroutine);
    }
}
