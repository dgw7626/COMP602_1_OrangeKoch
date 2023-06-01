/// <summary>
/// Moves the attached object back and forth between two specified positions.
/// </summary>
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform startPosition; // The starting position of the object
    public Transform endPosition; // The ending position of the object
    public float moveSpeed = 2f; // The speed at which the object moves

    private bool movingToEnd = true; // Flag to indicate if the object is moving towards the end position

    private void Update()
    {
        if (movingToEnd)
        {
            // Move the object towards the end position
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, moveSpeed * Time.deltaTime);
            
            // Check if the object has reached the end position
            if (Vector3.Distance(transform.position, endPosition.position) < 0.01f)
            {
                movingToEnd = false; // Reverse the direction of movement
            }
        }
        else
        {
            // Move the object towards the start position
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, moveSpeed * Time.deltaTime);
            
            // Check if the object has reached the start position
            if (Vector3.Distance(transform.position, startPosition.position) < 0.01f)
            {
                movingToEnd = true; // Reverse the direction of movement
            }
        }
    }
}
