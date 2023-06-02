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

        float damageValue = -50.0f;
        damageInfo.dmgValue = damageValue;
        float expectedHealth = playerHealth.currentHealth - damageValue;
        // ACT
        playerHealth.TakeDamage(damageInfo);

        // ASSERT
        Assert.AreEqual(expectedHealth, playerHealth.currentHealth);
    }

    [UnityTest]
    /// <summary>
    /// Coroutine that updates the UI health value over time.
    /// </summary>
    /// <returns>An IEnumerator used for the coroutine.</returns>
    public IEnumerator UpdateUI_HealthDecreasesOverTime()
    {
        // Set the current UI health value
        playerHealth.currentUIHealth = playerHealth.maxHealth;

        // Set the wait time for the coroutine
        playerHealth.UI_HealthTime = 0.16f;

        // Start the coroutine
        updateUICoroutine = playerHealth.StartCoroutine(playerHealth.UpdateUI());

        float elapsedTime = 0f;
        while (elapsedTime < playerHealth.UI_HealthTime + 0.1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Get the updated current UI health value
        float updatedUIHealth = playerHealth.currentUIHealth;

        // Validate if the health value has decreased, indicating that the UI has been updated
        Assert.Less(updatedUIHealth, 100.0f);

        // Stop the coroutine
        playerHealth.StopCoroutine(updateUICoroutine);
    }
}
