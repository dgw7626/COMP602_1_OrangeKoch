using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour, IWeaponFireable
{
    internal GameObject _hitObject;
    internal GameObject _bulletObject;
    internal GameObject _shellObject;
    internal Coroutine _currentCoroutine;
    
    public void Fire(Transform origin)
    {
        GuardClause.InspectGuardClauseNullRef<Transform>(origin, nameof(origin));
        transform.position = origin.position;
        var muzzleVfx = transform.Find(transform.ToString().Substring(0,3)+ "MuzzleFlash(Clone)").transform;
        var bulletVfx = transform.Find(transform.ToString().Substring(0, 3) + "BulletTracer(Clone)").transform;
        this._bulletObject = bulletVfx.gameObject;
        GuardClause.InspectGuardClauseNullRef<GameObject>(this._bulletObject, nameof(this._bulletObject));
        this._bulletObject.transform.position = origin.position;
        var hitVfx = transform.Find(transform.ToString().Substring(0, 3) + "hitObjects").transform;
        this._hitObject = hitVfx.gameObject;
        GuardClause.InspectGuardClauseNullRef<GameObject>(this._hitObject, nameof(this._hitObject));
        var shellObj = transform.Find(transform.ToString().Substring(0, 3) + "Shell(Clone)").transform;
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
                //Edit by Corey Knight - 04/05/23
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
