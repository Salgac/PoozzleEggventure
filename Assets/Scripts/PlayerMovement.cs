using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 1.0f;
    private float rotation_speed = 1.0f;
    private float movingPercentage = 0.0f;
    private float correctionDelta;

    private Boolean standing = true;
    private Vector3 direction;
    private KeyCode StandUpKey;

    private Vector3 lastPosition;

    private List<KeyCode> KeyBuffer = new List<KeyCode>();

    // Update is called once per frame
    void Update()
    {
        
        // set direction vector
        if (Input.GetKeyDown(KeyCode.W))
        {
            KeyBuffer.Add(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyBuffer.Add(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyBuffer.Add(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            KeyBuffer.Add(KeyCode.D);
        }
        
        // 2 types of movement
        if (standing && KeyBuffer.Count > 0)
        {   
            LayDown();
        }
        if (!standing && KeyBuffer.Count > 0)
        {
            // decide whether to roll or stand up
            Boolean AcceptKey = false;
            Vector3 state = transform.rotation.eulerAngles;
            if (state.z != 0 && (KeyBuffer[0] == KeyCode.W || KeyBuffer[0] == KeyCode.S))
            {
                Roll();
                AcceptKey = true;
            }
                
            if (state.x != 0 && (KeyBuffer[0] == KeyCode.A || KeyBuffer[0] == KeyCode.D))
            {
                Roll();
                AcceptKey = true;
            }
            if (!AcceptKey)
            {
                if (StandUp())
                    AcceptKey = true;
            }
                


            if (!AcceptKey) KeyBuffer.Clear();
        }

        

    }
    private void Roll()
    {
        Debug.Log(KeyBuffer.Count);
        if (KeyBuffer.Count > 0)
        {
            switch (KeyBuffer[0])
            {
                case (KeyCode.W):
                    direction = new Vector3(0,0,1.0f);
                    break;
                case (KeyCode.S):
                    direction = new Vector3(0, 0, -1.0f);
                    break;
                case (KeyCode.A):
                    direction = new Vector3(-1.0f, 0, 0);
                    break;
                case (KeyCode.D):
                    direction = new Vector3(1.0f, 0, 0);
                    break;
            }
            if (movingPercentage + Time.deltaTime >= 1.0f)
            {
                correctionDelta = 1.0f - movingPercentage;
                transform.position = transform.position + speed * direction * correctionDelta;
                movingPercentage += Time.deltaTime * speed;
            }
            else
            {   
                if (movingPercentage == 0.0f)
                {
                    lastPosition = transform.position;
                }
                movingPercentage += Time.deltaTime * speed;
                transform.position = transform.position + speed * direction * Time.deltaTime;
            }
        }
        if (movingPercentage >= 1.0f)
        {
            movingPercentage = 0.0f;
            // KeyBuffer.RemoveAt(0);
            // round to not fall from grid - proprably already solved elsewhere?
            float x = Convert.ToInt32(transform.position.x * 10) / 10.0f;
            float y = Convert.ToInt32(transform.position.y * 10) / 10.0f;
            float z = Convert.ToInt32(transform.position.z * 10) / 10.0f;
            transform.position = new Vector3(x, y, z);
        }
    }

    private void LayDown()
    {
        switch (KeyBuffer[0])
        {
            case (KeyCode.W):
                StandUpKey = KeyCode.S; 
                transform.Rotate(90,0,0);
                break;
            case (KeyCode.S):
                StandUpKey = KeyCode.W;
                transform.Rotate(-90, 0, 0);
                break;
            case (KeyCode.A):
                StandUpKey = KeyCode.D;
                transform.Rotate(0, 0, 90);
                break;
            case (KeyCode.D):
                StandUpKey = KeyCode.A;
                transform.Rotate(0, 0, -90);
                break;
        }
        standing = false;
        KeyBuffer.RemoveAt(0);
    }
    private Boolean StandUp()
    {
        if (KeyBuffer[0] == StandUpKey) {
            transform.rotation = Quaternion.identity;
            standing = true;
            KeyBuffer.RemoveAt(0);
            return true;
        }
        return false;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        KeyBuffer.Clear();
        transform.position = lastPosition;
        movingPercentage = 0.0f;

    }

}