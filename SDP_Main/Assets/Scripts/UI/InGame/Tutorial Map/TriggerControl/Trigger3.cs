/*

 ************************************************
 *                                              *
 * Primary Dev: 	Siyi Wang		            *
 * Student ID: 		19036757		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

 */
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

    /// <summary>
    /// Called when a collider enters the trigger.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger work");
        if (other.CompareTag("Player"))
        {
            //Move downward the tip text
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 100);
            JumpText.rectTransform.anchoredPosition -= new Vector2(0, 100);
            SprintText.rectTransform.anchoredPosition -= new Vector2(0, 100);
            //Strikethrough the tip
            MoveText.fontStyle |= FontStyles.Strikethrough;
            JumpText.fontStyle |= FontStyles.Strikethrough;
            SprintText.fontStyle |= FontStyles.Strikethrough;

            // Show the Sprint text
            ShootText.gameObject.SetActive(true);
            // disable the trigger.
            gameObject.SetActive(false);
        }
    }
}
