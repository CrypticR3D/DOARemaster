using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevRoomSpawn : MonoBehaviour
{
    public Transform Destination;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Instantiate(player, Destination, player.transform.rotation);
        }
    }
}
