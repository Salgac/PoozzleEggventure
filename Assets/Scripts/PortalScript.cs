using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private Vector3 OtherPortalLocation;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] OtherPortal;
        OtherPortal = GameObject.FindGameObjectsWithTag("Portal");
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(OtherPortal.Length);
        Debug.Log(player.transform.position);
        foreach (GameObject i in OtherPortal)
        {
            if (gameObject != i)
            {
                OtherPortalLocation = i.transform.position;
            }
        }
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Need to be teleported");
        Debug.Log(player.GetComponentInParent<PlayerMovement>());
        if (player.GetComponentInParent<PlayerMovement>().DisabledPortal == 0)
        {
            player.GetComponentInParent<PlayerMovement>().NeedToTeleport = true;
            player.GetComponentInParent<PlayerMovement>().TeleportTo = OtherPortalLocation;
        }
        

    }
}
