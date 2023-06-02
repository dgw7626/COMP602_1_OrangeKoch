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
using UnityEngine;
using TMPro;

public class Trigger2 : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI SprintText;
    public TextMeshProUGUI JumpText;
    public GameObject trigger3;
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
            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            JumpText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            MoveText.fontStyle |= FontStyles.Strikethrough;
            JumpText.fontStyle |= FontStyles.Strikethrough;
            // Show the Sprint text
            SprintText.gameObject.SetActive(true);
            // Set the object active
            trigger3.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
