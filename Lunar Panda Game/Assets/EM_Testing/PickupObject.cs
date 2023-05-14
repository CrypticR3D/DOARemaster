using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    // Variables for storing the object being picked up and the distance between the object and the player
    private GameObject pickedUpObject;
    private float distance;

    // Variables for the force applied to the object and the distance at which the object can be picked up
    public float force = 10f;
    public float distanceToPickup = 2f;

    void Update()
    {
        // Check if the player presses the "Interact" button (usually left mouse button)
        if (Input.GetButtonDown("Interact"))
        {
            // Raycast from the player's position in the direction they are facing
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // If the raycast hits an object within the distanceToPickup range
            if (Physics.Raycast(ray, out hit, distanceToPickup))
            {
                // Store the object hit by the raycast and the distance between the object and the player
                pickedUpObject = hit.collider.gameObject;
                distance = hit.distance;
            }
        }

        // If the player is holding an object
        if (pickedUpObject != null)
        {
            // Apply a force to the object to move it towards the player
            pickedUpObject.GetComponent<Rigidbody>().AddForce((transform.position - pickedUpObject.transform.position) * force);

            // Check if the distance between the object and the player is less than or equal to the distanceToPickup value
            if (Vector3.Distance(transform.position, pickedUpObject.transform.position) > distanceToPickup)
            {
                // Release the object if the distance is greater than the distanceToPickup value
                pickedUpObject = null;
            }
        }
    }
}
