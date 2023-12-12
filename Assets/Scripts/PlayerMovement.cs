using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

 /*
  * Class handling player movement
  */
public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    private float speed = 2.0f;

    private bool standing = true;
    private KeyCode StandUpKey;

    private Vector3 lastPosition = new Vector3(-0.5f, 1.5f, 0.5f);
    private Vector3 lastRotation = new Vector3(0, 0, 0);
    private Quaternion lastEggRotation;

    private List<KeyCode> KeyBuffer = new List<KeyCode>();

    // Animation variables
    public float transitionTime = 0.5f;
    private bool isTransitioning = false;

    // References variables
    public GameObject thiccAssVajicko;
    private MovementController movementBar;
    private List<GameObject> floorList;

    // Teleport variables
    public bool NeedToTeleport = false;
    public Vector3 TeleportTo;
    public int DisabledPortal = 0;

    // Set all needed variables - finding objects...
    void Start()
    {
        movementBar = FindObjectOfType<MovementController>();
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
        // set direction vector based on input and add to key buffer
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            KeyBuffer.Add(KeyCode.W);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            KeyBuffer.Add(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            KeyBuffer.Add(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            KeyBuffer.Add(KeyCode.D);
        }

        // Perform animation
        if (!isTransitioning) {
            // Teleport
            if (NeedToTeleport)
            {
                transform.position = TeleportTo;
                NeedToTeleport = false;
                TeleportTo = Vector3.zero;
                DisabledPortal = 1;
            }
            // 2 types of movement
            // Lay down
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
                // Perform roll animation
                if (state.z != 0 && (KeyBuffer[0] == KeyCode.W || KeyBuffer[0] == KeyCode.S))
                {
                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    lastEggRotation = thiccAssVajicko.transform.rotation;
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                // perform roll animation    
                if (state.x != 0 && (KeyBuffer[0] == KeyCode.A || KeyBuffer[0] == KeyCode.D))
                {
                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    lastEggRotation = thiccAssVajicko.transform.rotation;
                    StartCoroutine(Rolling());
                    AcceptKey = true;
                }
                // If no key is accepted then key buffer is reseted
                if (!AcceptKey)
                {
                    // Try to stand up
                    if (KeyBuffer[0] == StandUpKey)
                    {
                        lastPosition = transform.position;
                        lastRotation = transform.rotation.eulerAngles;
                        lastEggRotation = thiccAssVajicko.transform.rotation;
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

    // Rolling animation
    private IEnumerator Rolling()
    {
        // Prepare variables
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        Quaternion startEggRotation = thiccAssVajicko.transform.rotation;
        Quaternion endEggRotation = thiccAssVajicko.transform.rotation;

        isTransitioning = true;
        
        // Calculate final position and rotation
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

            // Coroutine cycle
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                thiccAssVajicko.transform.rotation = Quaternion.Lerp(startEggRotation, endEggRotation, t);
                yield return null;
            }

            // Reset variables
            isTransitioning = false;
            transform.position = endPosition;
            DisabledPortal = 0;
            CheckUnder();
        }
    }

    // Lay down animation
    private IEnumerator LayDown()
    {
        // Prepare variables
        isTransitioning = true;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = startPosition;
        Quaternion endRotation = Quaternion.identity;

        // Calculate final position
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

        // Coroutine cycle
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // Reset variables
        transform.position = endPosition;
        transform.rotation = endRotation;
        standing = false;
        if (KeyBuffer.Count > 0)
        {
            movementBar.IncrementProgress();
            KeyBuffer.RemoveAt(0);
        }
        isTransitioning = false;
        CheckUnder();
        DisabledPortal = 0;
    }

    // Stand up coroutine
    private IEnumerator StandUp()
    {
        isTransitioning = true;   
        if (KeyBuffer[0] == StandUpKey) {

            // Prepare variables
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            Vector3 endPosition = startPosition;
            Quaternion endRotation = Quaternion.identity;   

            // Calculate final postion
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
            
            // Coroutine cycle
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }

            // Reset variables
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
        CheckUnder();
        DisabledPortal = 0;
        yield return false;
    }

    // Return to last position Coroutine
    private IEnumerator ReturnToLastPosition()
    {
        // Variable set
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

        // Set final variables after animation
        isTransitioning = false;
        transform.position = endPosition;
        transform.rotation = endRotation;
        DisabledPortal = 0;
        KeyBuffer.Clear();
    }

    // Object collisions with triggers
    public void OnTriggerEnter(Collider other)
    {
        // check for collidable objects
        if (!other.CompareTag("Collidable"))
        {       
            return;
        }
        // Check layer of the object
        if (other.gameObject.transform.position.y >= 1)
        {
            Debug.Log(other.gameObject.name);
            // Check for active animations
            if (isTransitioning)
            {
                // Stop animation
                StopAllCoroutines();
                movementBar.IncrementProgress();
                isTransitioning = false;
            }

            // Return to last position before collision
            StartCoroutine(ReturnToLastPosition());

            // Reset standing variable
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

    // Restart current scene
    public IEnumerator Reset()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
        {
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Checking tile under the egg
    private void CheckUnder()
    {
        // FInding tile game object under player
        var under = floorList.Where(i => (i.transform.position.x == transform.position.x && i.transform.position.z == transform.position.z)).ToArray();
        
        // If no tile is discovered under player drop the egg     
        if (under.Length <= 0)
        {
            StopAllCoroutines();
            KeyBuffer.Clear();
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(Reset());
        }
            
        
    }
}
