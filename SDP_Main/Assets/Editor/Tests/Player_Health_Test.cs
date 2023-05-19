using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Player_Health_Test
{
    [Test]
    public void Test()
    {
        int a = 3;
        int b = 4;
        int c = 12;

        // ACT
        int Cal = a * b;

        // ARRANGE
        Assert.That(Cal,Is.EqualTo(c));
        // ASSERT
    }
} // {
//     public Player_Health playerHealth;
//     void Start(){
//         playerHealth =
//     }
//     [SetUp]
//     public void Setup()
//     {
//         // Initialize the PlayerHealth component
//         GameObject player = new GameObject();
//         playerHealth = player.AddComponent<PlayerHealth>();
//     }

//     [Test]
//     public void TakeDamage_DecreasesHealthByDamageAmount()
//     {
//         // Arrange
//         float initialHealth = playerHealth.currentHealth;
//         float damage = 25f;

//         // Act
//         playerHealth.TakeDamage(damage);

//         // Assert
//         Assert.AreEqual(initialHealth - damage, playerHealth.currentHealth);
//     }
// }
