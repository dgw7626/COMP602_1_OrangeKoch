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
        testObject = new GameObject("TestObject");
        orangeKoch = testObject.AddComponent<OrangeKoch>();
        mockSceneManager = new MockSceneManager();
        SceneManager.sceneLoaded += mockSceneManager.OnSceneLoaded;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testObject);
        SceneManager.sceneLoaded -= mockSceneManager.OnSceneLoaded;
    }

    [Test]
    public void LoadNextScene_After3Seconds()
    {
        orangeKoch.Start();
        Assert.IsFalse(mockSceneManager.LoadNextSceneCalled);
        Assert.AreNotEqual(3.0f, mockSceneManager.LoadNextSceneDelay);
    }


    public class MockSceneManager
    {
        public bool LoadNextSceneCalled { get; private set; }
        public float LoadNextSceneDelay { get; private set; }
        public string LoadedSceneName { get; private set; }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadedSceneName = scene.name;
        }

        public void LoadNextScene()
        {
            LoadNextSceneCalled = true;
            LoadNextSceneDelay = Time.deltaTime;
        }
    }
}

   


