// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;


// [TestFixture]
// public class PlayerHealthTests
// {
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