using NUnit.Framework;
using UnityEngine;

public class Player_Health_Tests
{
    private Player_Health playerHealth;


    [SetUp]
    public void Setup()
    {
        GameObject playerObject = new GameObject();
        playerHealth = playerObject.AddComponent<Player_Health>();
        playerHealth.maxHealth = 100f;
        playerHealth.currentHealth = playerHealth.maxHealth;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerHealth);
    }

    [Test]
    public void TakeDamageTest()
    {
        // ARRANGE
        s_DamageInfo damageInfo = new s_DamageInfo();

        float damageValue = -50.0f;
        damageInfo.dmgValue = damageValue;
        float expectedHealth = playerHealth.currentHealth - damageValue;
        // ACT
        playerHealth.TakeDamage(damageInfo);

        // ASSERT
        Assert.AreEqual(expectedHealth, playerHealth.currentHealth);
        
    }
}
