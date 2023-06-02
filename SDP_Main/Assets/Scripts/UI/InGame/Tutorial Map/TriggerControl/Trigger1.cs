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

public class Trigger1 : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI JumpText;
    public GameObject trigger2;

    /// <summary>
    /// Called when a collider enters the trigger.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the tip text downward
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 100);
            MoveText.fontStyle |= FontStyles.Strikethrough;
            // Show the Jump text
            JumpText.gameObject.SetActive(true);

            trigger2.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
