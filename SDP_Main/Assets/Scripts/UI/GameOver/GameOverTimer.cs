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


public class GameOverTimer : MonoBehaviour
{
    public float startTime = 120f;
    public float currentTime = 0;
    [SerializeField] Text textColoum;
    [SerializeField] Text textColor;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        textColoum.text = "Time " + currentTime.ToString("000");

        Color red = Color.red;

        if (currentTime <= 5)
        {
            textColoum.color = red;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
        }
    }


}
