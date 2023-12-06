using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;

    private bool standing = true;
    private KeyCode StandUpKey;

    private Vector3 lastPosition = new Vector3(-0.5f, 1.5f, 0.5f);
    private Vector3 lastRotation = new Vector3(0, 0, 0);

    private List<KeyCode> KeyBuffer = new List<KeyCode>();

    public float transitionTime = 0.5f;
    private bool isTransitioning = false;

    private GameObject thiccAssVajicko;
    private MovementBar movementBar;

    void Start()
    {
        thiccAssVajicko = transform.Find("Thicc_ass_vajicko").gameObject;
        movementBar = FindObjectOfType<MovementBar>();
    }

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
        
        if (!isTransitioning) {
            // 2 types of movement
            if (standing && KeyBuffer.Count > 0)
            {
                lastPosition = transform.position;
                lastRotation = transform.rotation.eulerAngles;
                StartCoroutine(LayDown());
            }
            if (!standing && KeyBuffer.Count > 0)
            {
                // decide whether to roll or stand up
                Boolean AcceptKey = false;
                Vector3 state = transform.rotation.eulerAngles;
                if (state.z != 0 && (KeyBuffer[0] == KeyCode.W || KeyBuffer[0] == KeyCode.S))
                {
                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                    
                if (state.x != 0 && (KeyBuffer[0] == KeyCode.A || KeyBuffer[0] == KeyCode.D))
                {
                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                if (!AcceptKey)
                {
                    if (KeyBuffer[0] == StandUpKey)
                    {
                        lastPosition = transform.position;
                        lastRotation = transform.rotation.eulerAngles;
                        StartCoroutine(StandUp());
                    }
                    else
                    {
                        movementBar.IncrementProgress();
                        KeyBuffer.RemoveAt(0);
                    }
                }
            }
        }
    }

    private IEnumerator Rolling()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = startPosition;
        Quaternion endRotation = Quaternion.identity;
        isTransitioning = true;
        if (KeyBuffer.Count > 0)
        {
            float rotationAmount = speed;
            switch (KeyBuffer[0])
            {
                case (KeyCode.W):
                    endPosition += new Vector3(0, 0, 1.0f);                    
                    break;
                case (KeyCode.S):
                    endPosition += new Vector3(0, 0, -1.0f);
                    break;
                case (KeyCode.A):
                    endPosition += new Vector3(-1.0f, 0, 0);
                    break;
                case (KeyCode.D):
                    endPosition += new Vector3(1.0f, 0, 0);
                    break;
            }

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                switch (KeyBuffer[0])
                {
                    case (KeyCode.W):
                        thiccAssVajicko.transform.Rotate(rotationAmount, 0, 0, Space.World);
                        break;
                    case (KeyCode.S):
                        thiccAssVajicko.transform.Rotate(-rotationAmount, 0, 0, Space.World);
                        break;
                    case (KeyCode.A):
                        thiccAssVajicko.transform.Rotate(0, 0, rotationAmount, Space.World);
                        break;
                    case (KeyCode.D):
                        thiccAssVajicko.transform.Rotate(0, 0, -rotationAmount, Space.World);
                        break;
                }
                yield return null;
            }
            isTransitioning = false;
            transform.position = endPosition;
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
            movementBar.IncrementProgress();
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
                movementBar.IncrementProgress();
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
        // LayDown into object hotfix? or Fix?
        if (other.gameObject.transform.position.y >= 1)
        {
            Debug.Log(other.gameObject.name);
            if (isTransitioning)
            {
                StopAllCoroutines();
                movementBar.IncrementProgress();
                isTransitioning = false;
            }

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
            KeyBuffer.Clear();

        } 
    }
}
