using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Player_Health_Tests
{
    private Player_Health playerHealth;
    private Coroutine updateUICoroutine;

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
        Object.DestroyImmediate(playerHealth.gameObject);
    }

    [Test]
    public void TakeDamageTest()
    {
        // ARRANGE
        s_DamageInfo damageInfo = new s_DamageInfo();

        float damageValue = 50.0f;
        damageInfo.dmgValue = damageValue;
        float expectedHealth = playerHealth.currentHealth - damageValue;
        // ACT
        playerHealth.TakeDamage(damageInfo);

        // ASSERT
        Assert.AreEqual(expectedHealth, playerHealth.currentHealth);
    }

    [UnityTest]
    public IEnumerator UpdateUI_HealthDecreasesOverTime()
    {
        // ARRANGE
        playerHealth.currentUIHealth = playerHealth.maxHealth;
        playerHealth.UI_HealthTime = 0.16f;
        // ACT
        updateUICoroutine = playerHealth.StartCoroutine(playerHealth.UpdateUI());

        float elapsedTime = 0f;
        while (elapsedTime < playerHealth.UI_HealthTime + 0.1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        float updatedUIHealth = playerHealth.currentUIHealth;

        // ASSERT
        Assert.Less(updatedUIHealth, 100.0f);

        // Stop the coroutine
        playerHealth.StopCoroutine(updateUICoroutine);
    }

    [Test]
    public void Respawn_RestoresHealthToMax()
    {
        // Arrange
        Player_Health playerHealth = new Player_Health();
        float maxHealth = 100f;
        float initialHealth = 50f;

        // Set the initial health
        playerHealth.maxHealth = maxHealth;
        playerHealth.currentHealth = initialHealth;
        playerHealth.currentUIHealth = initialHealth;

        // Act
        playerHealth.Respawn();

        // Assert
        Assert.AreEqual(maxHealth, playerHealth.currentHealth);
        Assert.AreEqual(maxHealth, playerHealth.currentUIHealth);
    }
}
