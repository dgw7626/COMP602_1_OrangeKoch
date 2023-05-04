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
        if(!Game_RuntimeData.isMultiplayer)
        {
            moveText = transform.Find("MoveText").GetComponent<TextMeshProUGUI>();
            jumpText = transform.Find("JumpText").GetComponent<TextMeshProUGUI>();
            // Show the movement tip text at beginning
            transform.parent.gameObject.SetActive(true);
            moveText.gameObject.SetActive(true);
            jumpText.gameObject.SetActive(false);

        }
    }
}
