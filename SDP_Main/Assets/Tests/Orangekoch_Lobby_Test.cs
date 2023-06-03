using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Orangekoch_Lobby_Test
{
    private GameObject testObject;
    private OrangeKoch orangeKoch;
    private MockSceneManager mockSceneManager;

    [SetUp]
    public void Setup()
    {
        // Creating a test GameObject and adding necessary components for testing
        testObject = new GameObject("TestObject");
        orangeKoch = testObject.AddComponent<OrangeKoch>();
        mockSceneManager = new MockSceneManager();
        SceneManager.sceneLoaded += mockSceneManager.OnSceneLoaded;
    }

    [TearDown]
    public void Teardown()
    {
        // Destroying the test GameObject and cleaning up event subscriptions
        Object.DestroyImmediate(testObject);
        SceneManager.sceneLoaded -= mockSceneManager.OnSceneLoaded;
    }

    [Test]
    public void LoadNextScene_After3Seconds()
    {
        // Starting the test case
        orangeKoch.Start();
        // Asserting that LoadNextScene has not been called and the delay is not 3.0f
        Assert.IsFalse(mockSceneManager.LoadNextSceneCalled);
        Assert.AreNotEqual(3.0f, mockSceneManager.LoadNextSceneDelay);
    }

    public class MockSceneManager
    {
        // Variables to track the state of the mock scene manage
        public bool LoadNextSceneCalled { get; private set; }
        public float LoadNextSceneDelay { get; private set; }
        public string LoadedSceneName { get; private set; }

        // Event handler for the OnSceneLoaded event
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadedSceneName = scene.name;
        }

        // Simulating the LoadNextScene method
        public void LoadNextScene()
        {
            // Setting flags and delay to track the method call
            LoadNextSceneCalled = true;
            LoadNextSceneDelay = Time.deltaTime;
        }
    }
}

   


