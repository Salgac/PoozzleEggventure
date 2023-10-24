using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 1.0f;
    private float movingPercentage = 0.0f;
    private float correctionDelta;
    private Vector3 direction = new Vector3(0, 0, 0);

    private List<Vector3> KeyBuffer= new List<Vector3>();


    // Update is called once per frame
    void Update()
    {
        
        // set direction vector
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            KeyBuffer.Add(new Vector3(0, 0, 1.0f));
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            KeyBuffer.Add(new Vector3(0, 0, -1.0f));
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            KeyBuffer.Add(new Vector3(-1.0f, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            KeyBuffer.Add(new Vector3(1.0f, 0, 0));
        }
        // Testing
        // rotations - in testing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(transform.localRotation.x - 90,0,0);
        }

        // moving
        if (KeyBuffer.Count > 0)
        {
            direction = KeyBuffer[0];
            if (movingPercentage + Time.deltaTime >= 1.0f)
            {
                correctionDelta = 1.0f - movingPercentage;
                transform.position = transform.position + speed * direction * correctionDelta;
                movingPercentage += Time.deltaTime;
            }
            else
            {
                movingPercentage += Time.deltaTime;
                transform.position = transform.position + speed * direction * Time.deltaTime;
            } 
        }
        if (movingPercentage >= 1.0f)
        {
            movingPercentage = 0.0f;
            KeyBuffer.RemoveAt(0);
            // round to not fall from grid 
            float x = Convert.ToInt32(transform.position.x * 10) / 10.0f;
            float y = Convert.ToInt32(transform.position.y * 10) / 10.0f;
            float z = Convert.ToInt32(transform.position.z * 10) / 10.0f;
            transform.position = new Vector3(x, y, z);
        }
        

    }
}
