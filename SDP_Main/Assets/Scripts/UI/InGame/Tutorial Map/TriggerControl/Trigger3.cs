using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Trigger3 : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI JumpText;
    public TextMeshProUGUI SprintText;
    public TextMeshProUGUI ShootText;

    // public GameObject trigger3;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger work");
        if (other.CompareTag("Player"))
        {
            //Move downward the tip text
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            JumpText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            SprintText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            //Strikethrough the tip
            MoveText.fontStyle |= FontStyles.Strikethrough;
            JumpText.fontStyle |= FontStyles.Strikethrough;
            SprintText.fontStyle |= FontStyles.Strikethrough;

            // Show the Sprint text
            ShootText.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
