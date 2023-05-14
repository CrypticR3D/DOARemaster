using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent2Drawer : MonoBehaviour
{

    [SerializeField] public GameObject drawer;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Parent");
        if (other.tag == "InteractiveObject" )
        {
            other.transform.parent = drawer.transform;
            other.attachedRigidbody.useGravity = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Orphan");
        if (other.tag == "InteractiveObject")
        {
            other.attachedRigidbody.useGravity = true;
            other.transform.parent = null;
        }
    }
}
