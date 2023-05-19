// using System.Collections;

// using NUnit.Framework;
// using UnityEngine;


// public class PlayerHealthTests
// {
//     public PlayerHealth playerHealth;

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