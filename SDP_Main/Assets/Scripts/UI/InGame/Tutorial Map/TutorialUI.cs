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

/// <summary>
/// The TutorialUI class is responsible for display the first tip when game start
/// </summary>
public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI moveText;
    private void Start()
    {
        // Show the movement tip text at beginning
        moveText.gameObject.SetActive(true);
    }
}
