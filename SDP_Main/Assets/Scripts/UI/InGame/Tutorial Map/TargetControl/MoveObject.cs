using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition; 
    public float moveSpeed = 2f;

    private bool movingToEnd = true;


    private void Update()
    {
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPosition.position) < 0.01f)
            {
                movingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition.position) < 0.01f)
            {
                movingToEnd = true;
            }
        }
    }
}