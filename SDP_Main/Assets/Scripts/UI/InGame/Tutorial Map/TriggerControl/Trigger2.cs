using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Trigger2 : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI SprintText;
    public TextMeshProUGUI JumpText;
    public GameObject trigger3;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger work");
        if (other.CompareTag("Player"))
        {
            //Move downward the tip text
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            JumpText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            MoveText.fontStyle |= FontStyles.Strikethrough;
            JumpText.fontStyle |= FontStyles.Strikethrough;
            // Show the Sprint text
            SprintText.gameObject.SetActive(true);
            
            trigger3.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
