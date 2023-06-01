using UnityEngine;
using System.Collections;

public class TargetRespawn : MonoBehaviour
{
    public float respawnTime = 3f; // spawn time
    public Vector3 respawnPosition; // spawn position

    private bool isRespawning = false;

    private void Start()
    {
        // Set the initial spawn position to the current position
        respawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRespawning)
        {
            //Destroy(gameObject);
            RespawnDelay();
        }
    }

    public void RespawnDelay()
    {
        isRespawning = true;
        // Hide object
        gameObject.SetActive(false);
        Debug.Log("2s");
        // Wait for a moment before activating the object
        Invoke(nameof(Respawn), respawnTime);
        Debug.Log("3s");
    }

    private void Respawn()
    {
        Debug.Log("3.5s");

        // Set the spawn position for new objects
        transform.position = respawnPosition;
        Debug.Log("4s");

        // Activate the object
        gameObject.SetActive(true);
        Debug.Log("5s");

        isRespawning = false;
    }
}
