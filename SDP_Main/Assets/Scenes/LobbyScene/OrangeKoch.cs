using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using UnityEngine.SceneManagement;
using TMPro;


public class OrangeKoch : MonoBehaviour
{
    public string Main_Menu = "MainMenuUI";

    // public float targetFontSize = 100.0f;
    // public float speed = 10f;

    // private TextMeshPro textComponent;

    // Start is called before the first frame update
    void Start()
    {
        //textComponent = GetComponent<TextMeshPro>();

        Invoke("LoadNextScene", 5f);
    }

    // Update is called once per frame
    // void update ()
    // {
    //     if (textComponent.fontSize < targetFontSize)
    //     {
    //         textComponent.fontSize += (int)(speed * Time.deltaTime);
    //     }
    // }
    void LoadNextScene()
    {
        SceneManager.LoadScene(Main_Menu);
    }
}


