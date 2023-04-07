
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed = 0.5f;
    public float ySpeed = 0.5f; 
    public float zSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(xSpeed,0,0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(xSpeed,0,0);
        }

        if (Input.GetKey(KeyCode.R))
        {
            transform.position += new Vector3(0,ySpeed,0);
        }

        if (Input.GetKey(KeyCode.F))
        {
            transform.position -= new Vector3(0,ySpeed,0);
        }

         if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0,0,zSpeed);
        }

         if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0,0,zSpeed);
        }
    }
}
