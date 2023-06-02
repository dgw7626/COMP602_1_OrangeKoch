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
public class PlayerUI_HealthBar : MonoBehaviour
{
    public Slider slider; 
    public Gradient gradient; 
    public Image fill; 

    /// <summary>
    /// Sets the maximum health value of the health bar and updates the fill color.
    /// </summary>
    /// <param name="health">The maximum health value.</param>
    public void SetMaxHealth(float health)
    {
        // Set the maximum value of the slider
        slider.maxValue = health;
        // Set the current value of the slider to the maximum value
        slider.value = health;
        // Set the fill color using the gradient at the maximum value
        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// Updates the current health value of the health bar and updates the fill color.
    /// </summary>
    /// <param name="health">The current health value.</param>
    public void SetHealth(float health)
    {
        if (slider != null && fill != null)
        {
            // Set the current value of the slider to the provided health value
            slider.value = health;
            // Set the fill color using the gradient at the normalized value of the slider
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
