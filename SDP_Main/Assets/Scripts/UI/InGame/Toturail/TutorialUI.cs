using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI jumpText;
    private void Start()
    {
        // Show the movement tip text at beginning
        moveText.gameObject.SetActive(true);
        jumpText.gameObject.SetActive(false);
    }
}
