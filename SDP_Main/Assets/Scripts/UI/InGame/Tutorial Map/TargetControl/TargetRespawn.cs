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
using System.Collections;

public class TargetRespawn : MonoBehaviour
{
    public float respawnTime = 3f; // The time it takes for the target to respawn
    public Vector3 respawnPosition; // The position where the object will respawn
    private bool isRespawning = false; // Indicates if the object is currently respawning
    public ScoreCount scoreCount; // Reference to the ScoreCount script

    /// <summary>
    /// Sets the initial spawn position to the current position and finds the ScoreCount script in the scene.
    /// </summary>
    private void Start()
    {
        respawnPosition = transform.position;
        scoreCount = FindObjectOfType<ScoreCount>();
    }

    /// <summary>
    /// Initiates the delay before respawning the object.
    /// </summary>
    public void RespawnDelay()
    {
        isRespawning = true;
        gameObject.SetActive(false); // Hide the object
        Invoke(nameof(Respawn), respawnTime); // Wait for a moment before activating the object
    }

    /// <summary>
    /// Respawns the object by setting its position to the respawn position and activating it.
    /// </summary>
    private void Respawn()
    {
        transform.position = respawnPosition; // Set the spawn position for new objects
        gameObject.SetActive(true); // Activate the target
        isRespawning = false;
    }
}
