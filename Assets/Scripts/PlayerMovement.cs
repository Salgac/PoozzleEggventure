using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    private float rotation_speed = 1.0f;
    private float movingPercentage = 0.0f;
    private float correctionDelta;

    private bool standing = true;
    private Vector3 direction;
    private KeyCode StandUpKey;

    private Vector3 lastPosition = new Vector3(-0.5f, 1.5f, 0.5f);
    private Vector3 lastRotation = new Vector3(0, 0, 0);

    private List<KeyCode> KeyBuffer = new List<KeyCode>();

    public float transitionTime = 0.5f;
    private bool isTransitioning = false;

    // Update is called once per frame
    void Update()
    {    
        // set direction vector
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Standing: " + standing + ", isTransitioning: " + isTransitioning + ", KeyBuffer Count: " + KeyBuffer.Count);
            KeyBuffer.Add(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Standing: " + standing + ", isTransitioning: " + isTransitioning + ", KeyBuffer Count: " + KeyBuffer.Count);
            KeyBuffer.Add(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Standing: " + standing + ", isTransitioning: " + isTransitioning + ", KeyBuffer Count: " + KeyBuffer.Count);
            KeyBuffer.Add(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Standing: " + standing + ", isTransitioning: " + isTransitioning + ", KeyBuffer Count: " + KeyBuffer.Count);
            KeyBuffer.Add(KeyCode.D);
        }
        
        if (!isTransitioning) {
            // 2 types of movement
            if (standing && KeyBuffer.Count > 0)
            {   
                StartCoroutine(LayDown());
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
                    StartCoroutine(StandUp());
                }
            }
        }
    }

    private void Roll()
    {
        //Debug.Log(KeyBuffer.Count);
        if (KeyBuffer.Count > 0)
        {
            switch (KeyBuffer[0])
            {
                case (KeyCode.W):
                    direction = new Vector3(0, 0, 1.0f);
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
                    lastRotation = transform.rotation.eulerAngles;
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

    private IEnumerator LayDown()
    {
        isTransitioning = true;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = startPosition;
        Quaternion endRotation = Quaternion.identity;

        switch (KeyBuffer[0])
        {
            case KeyCode.W:
                StandUpKey = KeyCode.S;
                endPosition += new Vector3(0, 0, 1);
                endRotation = Quaternion.Euler(90, 0, 0);
                break;
            case KeyCode.S:
                StandUpKey = KeyCode.W;
                endPosition += new Vector3(0, 0, -1);
                endRotation = Quaternion.Euler(-90, 0, 0);
                break;
            case KeyCode.A:
                StandUpKey = KeyCode.D;
                endPosition += new Vector3(-1, 0, 0);
                endRotation = Quaternion.Euler(0, 0, 90);
                break;
            case KeyCode.D:
                StandUpKey = KeyCode.A;
                endPosition += new Vector3(1, 0, 0);
                endRotation = Quaternion.Euler(0, 0, -90);
                break;
        }

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = endRotation;
        standing = false;
        if (KeyBuffer.Count > 0)
        {
            KeyBuffer.RemoveAt(0);
        }
        isTransitioning = false;
    }
    private IEnumerator StandUp()
    {
        isTransitioning = true;   
        if (KeyBuffer[0] == StandUpKey) {
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 endPosition = startPosition;
            Quaternion endRotation = Quaternion.identity;   
            switch (KeyBuffer[0])
            {
                case KeyCode.W:
                    endPosition += new Vector3(0, 0, 1);
                    break;
                case KeyCode.S:
                    endPosition += new Vector3(0, 0, -1);
                    break;
                case KeyCode.A:
                    endPosition += new Vector3(-1, 0, 0);
                    break;
                case KeyCode.D:
                    endPosition += new Vector3(1, 0, 0);
                    break;
            }
            
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }

            transform.position = endPosition;
            transform.rotation = endRotation;
            standing = true;
            if (KeyBuffer.Count > 0)
            {
                KeyBuffer.RemoveAt(0);
            }
        }
        isTransitioning = false; 
        yield return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        // check for collidable objects
        if (!other.CompareTag("Collidable"))
        {
            return;
        }

        KeyBuffer.Clear();
        //Debug.Log(other.gameObject.name);
        //Debug.Log(StandUpKey);
        //Debug.Log(standing);

        transform.position = lastPosition;
        transform.rotation = Quaternion.identity;
        transform.Rotate(lastRotation);

        if (lastRotation == new Vector3(0, 0, 0))
        {
            standing = true;
        }
        else
        {
            standing = false;
        }
        movingPercentage = 0.0f;
    }
}
