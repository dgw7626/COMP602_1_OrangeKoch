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
using UnityEngine.UI;

/// <summary>
/// class is responsible for updating ammunition UI when ammo number changes.
/// </summary>
public class AmmunitionUI : MonoBehaviour
{
    public Text Text; // Reference to the Text component

    /// <summary>
    /// Sets the ammunition count in the UI.
    /// </summary>
    /// <param name="currentBullet">The current bullet count.</param>
    /// <param name="bulletmag">The maximum bullet count.</param>
    public void SetAmmunition(int currentBullet, int bulletmag)
    {
        // Update the text with the current bullet count and maximum bullet count
        Text.text = currentBullet + " / " + bulletmag;
    }

    /// <summary>
    /// Updates the ammunition count in the UI.
    /// </summary>
    /// <param name="currentBullet">The current bullet count.</param>
    /// <param name="bulletmag">The maximum bullet count.</param>
    public void UpdateUI(int currentBullet, int bulletmag)
    {
        // Update the text with the current bullet count and maximum bullet count
        Text.text = currentBullet + " / " + bulletmag;
    }
}
