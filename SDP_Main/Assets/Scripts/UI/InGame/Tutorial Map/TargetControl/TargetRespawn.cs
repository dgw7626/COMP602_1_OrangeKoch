using UnityEngine;
using System.Collections;

public class TargetRespawn : MonoBehaviour
{
    public float respawnTime = 3f; // spawn time
    public Vector3 respawnPosition; // spawn position

    private bool isRespawning = false;

    private ScoreCount scoreCount;

    private void Start()
    {
        // Set the initial spawn position to the current position
        respawnPosition = transform.position;
        scoreCount = FindObjectOfType<ScoreCount>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRespawning)
        {
            //Destroy(gameObject);
            scoreCount.IncreaseScore();
            RespawnDelay();
        }
    }

    public void RespawnDelay()
    {
        isRespawning = true;
        // Hide object
        gameObject.SetActive(false);
        // Wait for a moment before activating the object
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        // Set the spawn position for new objects
        transform.position = respawnPosition;
        // Activate the object
        gameObject.SetActive(true);
        isRespawning = false;
    }
}
