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

    void Start()
    {
        Invoke("LoadNextScene", 3f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(Main_Menu);
    }
}


