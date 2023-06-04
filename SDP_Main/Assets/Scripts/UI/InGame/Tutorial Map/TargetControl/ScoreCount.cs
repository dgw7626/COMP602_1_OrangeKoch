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
using UnityEngine.SceneManagement;

/// <summary>
/// The ScoreCount class manages the player's score, quit time, score text, and confirmation window.
/// </summary>
public class ScoreCount : MonoBehaviour
{
    public float quitTime = 5f;
    public TextMeshPro scoreText; // Show the score text
    private int score = 0; // score
    public GameObject confirmation;
    /// <summary>
    /// Start is called before the first frame update.
    /// Initializes the score text.
    /// </summary>
    private void Start()
    {
        confirmation.SetActive(false);
        UpdateScoreText();
    }

    /// <summary>
    /// Increases the score by 1, 
    /// If the score is equal to or greater than 10, it shows a confirmation window.
    /// </summary>
    public void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        if (score >= 10)
        {
            if (confirmation != null)
            {
                ShowConfirmationWindow();
                Invoke(nameof(SwitchToScene), quitTime);
            }
        }
    }

    /// <summary>
    /// Updates the score text with the current score value.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Shows the confirmation window by setting its active state to true.
    /// </summary>
    public void ShowConfirmationWindow()
    {
        confirmation.SetActive(true);
    }

    /// <summary>
    /// Switches to the "MainMenuUI" scene.
    /// </summary>
    public void SwitchToScene()
    {
        SceneManager.LoadScene("MainMenuUI");
    }

}
