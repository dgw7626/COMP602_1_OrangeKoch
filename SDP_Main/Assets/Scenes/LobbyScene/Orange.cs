using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Orange : MonoBehaviour
{  
    public string Main_Menu = "MainMenuUI";

    public float targetFontSize = 100.0f;
    public float speed = 10f;

    public Text textComponent;
    // Start is called before the first frame update
    void Start()
    {
         textComponent = GetComponent<Text>();
         Invoke("LoadNextScene", 5f);
    }

    // Update is called once per frame
    void Update()
    {
         if (textComponent.fontSize < targetFontSize)
        {
            textComponent.fontSize += (int)(speed * Time.deltaTime);
        }
    }

    void LoadNextScene()
     {
         SceneManager.LoadScene(Main_Menu);
     }
}
