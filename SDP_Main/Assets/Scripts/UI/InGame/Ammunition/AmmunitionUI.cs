using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmunitionUI : MonoBehaviour
{
    // Start is called before the first frame update
  
    public Text Text;
    public void SetAmmunition(int currentBullet,int bulletmag)
    {
        Text.text = currentBullet + " / " + bulletmag;
        
    }


        public void UpdateUI(int currentBullet,int bulletmag)
    {
        Text.text = currentBullet + " / " + bulletmag;
    }
 
}
