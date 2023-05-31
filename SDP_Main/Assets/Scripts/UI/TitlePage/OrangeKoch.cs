using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SceneManagement;

public class OrangeKoch : MonoBehaviour
{
    public string Main_Menu = "Assets/Scenes/MainMenuUI.unity";
    public bool loadNextSceneInvoked = false;

    public void Start()
    {
        Invoke("LoadNextScene", 3f);
    }

    //public void LoadNextScene()
    //{
    //    loadNextSceneInvoked = true;
    //    //EditorSceneManager.OpenScene(Main_Menu);
    //    SceneManager.LoadScene(Main_Menu);
    //}

    public  void LoadNextScene()
    {
        loadNextSceneInvoked = true;
        SceneManager.LoadScene(Main_Menu);

//#if UNITY_EDITOR
//        EditorSceneManager.OpenScene(Main_Menu);
//    #else
//                SceneManager.LoadScene(Main_Menu);
//    #endif
    }


    public bool IsLoadNextSceneInvoked()
    {
        return loadNextSceneInvoked;
    }
}
