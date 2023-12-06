using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    private MovementBar movementBar;

    // Start is called before the first frame update
    void Start()
    {
        movementBar = FindObjectOfType<MovementBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //decrement progress 3 times
            movementBar.DecrementProgress(3);
            gameObject.SetActive(false);
        }
    }
}
