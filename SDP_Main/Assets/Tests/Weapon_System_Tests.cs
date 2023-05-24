using NUnit.Framework;
using UnityEngine;

namespace Editor
{
    public class Weapon_System_Tests:MonoBehaviour
    {
       public class CurrentPlayerDamageProperty
        {
            Weapon_Bullet weapon_bullet;
            public GameObject bullet_instance;
            [SetUp]
            public void InitSetup()
            {
                weapon_bullet = Instantiate(bullet_instance, Vector3.zero, Quaternion.identity).transform.GetComponent<Weapon_Bullet>();
            }

            [Test]
            public void Set_Damage_To_10()
            {
                float result = 10.0f;
                //setting up the initial value for the waepon damage.    
                weapon_bullet._weaponDamage = 10.0f;
                float currentDamage = weapon_bullet._weaponDamage;
                Assert.AreEqual(currentDamage, result);
          
            }
        }
    }
}
