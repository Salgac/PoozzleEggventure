using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    private MovementController movementBar;

    // Start is called before the first frame update
    void Start()
    {
        movementBar = FindObjectOfType<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movementBar.IncrementProgress(3);
            gameObject.SetActive(false);
        }
    }
}
