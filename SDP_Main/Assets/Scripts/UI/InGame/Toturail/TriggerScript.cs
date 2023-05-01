using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TriggerScript : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI JumpText;

    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger work");
        if (!triggered && other.CompareTag("Player"))
        {
            Debug.Log("Player work");
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            MoveText.fontStyle |= FontStyles.Strikethrough;
            // Show the Jump text
            JumpText.gameObject.SetActive(true);

            triggered = true;
            Destroy(gameObject);
        }
    }
}
