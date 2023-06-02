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

public class MoveObject : MonoBehaviour
{
    // The starting position of the object
    public Transform startPosition;

    // The ending position of the object
    public Transform endPosition;

    // The speed at which the object moves
    public float moveSpeed = 2f;

    // Flag to indicate if the object is moving towards the end position
    private bool movingToEnd = true;

    /// <summary>
    /// Moves the attached object back and forth between two specified positions.
    /// </summary>
    private void Update()
    {
        if (movingToEnd)
        {
            // Move the object towards the end position
            transform.position = Vector3.MoveTowards(
                transform.position,
                endPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Check if the object has reached the end position
            if (Vector3.Distance(transform.position, endPosition.position) < 0.01f)
            {
                // Reverse the direction of movement
                movingToEnd = false; 
            }
        }
        else
        {
            // Move the object towards the start position
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Check if the object has reached the start position
            if (Vector3.Distance(transform.position, startPosition.position) < 0.01f)
            {
                // Reverse the direction of movement
                movingToEnd = true;
            }
        }
    }
}
