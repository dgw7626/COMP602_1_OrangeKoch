using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialMap_ExitConfirmation : MonoBehaviour
{
    public GameObject confirmationPanel;
    // Start is called before the first frame update
    void Start()
    {
        // 禁用确认窗口
        confirmationPanel.SetActive(false);
    }

    // Update is called once per frame
    public void ShowConfirmationWindow()
    {
        // 启用确认窗口
        confirmationPanel.SetActive(true);
    }
}
