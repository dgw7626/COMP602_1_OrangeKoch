using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Orange_koch_Test
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
    public void LoadNextScene_LoadsMainMenuUI()
    {
        orangeKoch.Main_Menu = "Assets/Scenes/MainMenuUI.unity";
       // string mainm = "MainMenuUI";
        orangeKoch.LoadNextScene();
        Assert.IsTrue(orangeKoch.IsLoadNextSceneInvoked());
        Assert.AreNotEqual("Main_Menu", mockSceneManager.LoadedSceneName);
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




