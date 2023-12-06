using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 2.0f;

    private bool standing = true;
    private KeyCode StandUpKey;

    private Vector3 lastPosition = new Vector3(-0.5f, 1.5f, 0.5f);
    private Vector3 lastRotation = new Vector3(0, 0, 0);
    private Quaternion lastEggRotation;

    private List<KeyCode> KeyBuffer = new List<KeyCode>();

    public float transitionTime = 0.5f;
    private bool isTransitioning = false;

    private GameObject thiccAssVajicko;
    private List<GameObject> floorList;

    void Start()
    {
        thiccAssVajicko = transform.Find("Thicc_ass_vajicko").gameObject;
        GameObject[] floor = GameObject.FindGameObjectsWithTag("Collidable");
        floorList = floor.ToList();
        
        for (int i = floor.Length-1; i > 0; i--)
        {
            if (floor[i].transform.position.y > 0)
                floorList.RemoveAt(i);
        }
        floor = floorList.ToArray();
    }

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
                lastPosition = transform.position;
                lastRotation = transform.rotation.eulerAngles;
                lastEggRotation = thiccAssVajicko.transform.rotation;
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
                    lastEggRotation = thiccAssVajicko.transform.rotation;
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                    
                if (state.x != 0 && (KeyBuffer[0] == KeyCode.A || KeyBuffer[0] == KeyCode.D))
                {
                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    lastEggRotation = thiccAssVajicko.transform.rotation;
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                if (!AcceptKey)
                {
                    if (KeyBuffer[0] == StandUpKey)
                    {
                        lastPosition = transform.position;
                        lastRotation = transform.rotation.eulerAngles;
                        lastEggRotation = thiccAssVajicko.transform.rotation;
                        StartCoroutine(StandUp());
                    }
                    else
                    {
                        KeyBuffer.RemoveAt(0);
                    }
                }
            }
        }
    }

    private IEnumerator Rolling()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        Quaternion startEggRotation = thiccAssVajicko.transform.rotation;
        Quaternion endEggRotation = thiccAssVajicko.transform.rotation;

        isTransitioning = true;
        
        if (KeyBuffer.Count > 0)
        {
            switch (KeyBuffer[0])
            {
                case (KeyCode.W):
                    endPosition += new Vector3(0, 0, 1.0f);
                    thiccAssVajicko.transform.Rotate(90, 0, 0, Space.World);
                    endEggRotation = thiccAssVajicko.transform.rotation;
                    
                    break;
                case (KeyCode.S):
                    endPosition += new Vector3(0, 0, -1.0f);
                    thiccAssVajicko.transform.Rotate(-90, 0, 0, Space.World);
                    endEggRotation = thiccAssVajicko.transform.rotation;
                    break;
                case (KeyCode.A):
                    endPosition += new Vector3(-1.0f, 0, 0);
                    thiccAssVajicko.transform.Rotate(0, 0, 90, Space.World);
                    endEggRotation = thiccAssVajicko.transform.rotation;
                    break;
                case (KeyCode.D):
                    endPosition += new Vector3(1.0f, 0, 0);
                    thiccAssVajicko.transform.Rotate(0, 0, -90, Space.World);
                    endEggRotation = thiccAssVajicko.transform.rotation;
                    break;
            }
            thiccAssVajicko.transform.rotation = startEggRotation;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                thiccAssVajicko.transform.rotation = Quaternion.Lerp(startEggRotation, endEggRotation, t);
                yield return null;
            }
            isTransitioning = false;
            transform.position = endPosition;
            CheckUnder();
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
        CheckUnder();
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
        CheckUnder();
        yield return false;
    }

    private IEnumerator ReturnToLastPosition()
    {
        isTransitioning = true;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = lastPosition;
        Quaternion endRotation = Quaternion.identity;
        endRotation = Quaternion.Euler(lastRotation);

        Quaternion startEggRotation = thiccAssVajicko.transform.rotation;
        Quaternion endEggRotation = lastEggRotation;

        // Coroutine loop
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
        {
            // Internal egg rotation 
            thiccAssVajicko.transform.rotation = Quaternion.Lerp(startEggRotation, endEggRotation, t);
            // animation
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        isTransitioning = false;
        transform.position = endPosition;
        transform.rotation = endRotation;
        KeyBuffer.Clear();
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
                isTransitioning = false;
            }

            StartCoroutine(ReturnToLastPosition());
            // transform.position = lastPosition;
            // transform.rotation = Quaternion.identity;
            // transform.Rotate(lastRotation);

            if (lastRotation == new Vector3(0, 0, 0))
            {
                standing = true;
            }
            else
            {
                standing = false;
            }
            

        } 
    }

    public IEnumerator Reset()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
        {
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CheckUnder()
    {

        var under = floorList.Where(i => (i.transform.position.x == transform.position.x && i.transform.position.z == transform.position.z)).ToArray();
        Debug.Log(under.Length);
        if (under.Length <= 0)
        {
            StopAllCoroutines();
            KeyBuffer.Clear();
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(Reset());
        }
            
        
    }
}
