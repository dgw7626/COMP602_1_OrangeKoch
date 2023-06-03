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

using TMPro;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    public TextMeshPro scoreText; // Show the score text
    private int score = 0; // score

    /// <summary>
    /// Start is called before the first frame update.
    /// Initializes the score text.
    /// </summary>
    private void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// Increases the score by 1 and updates the score text.
    /// </summary>
    public void IncreaseScore()
    {
        score++;
        UpdateScoreText();
    }

    /// <summary>
    /// Updates the score text with the current score value.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
