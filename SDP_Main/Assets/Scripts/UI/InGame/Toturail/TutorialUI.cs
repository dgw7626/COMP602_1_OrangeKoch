using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI jumpText;

    public Text test;
    private bool hasMoved = false;

    private void Start()
    {
        // 开始时显示移动提示文本
        moveText.gameObject.SetActive(true);
        jumpText.gameObject.SetActive(false);
    }
}
