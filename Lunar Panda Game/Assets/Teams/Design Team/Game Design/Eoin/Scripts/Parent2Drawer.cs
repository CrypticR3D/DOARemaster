using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent2Drawer : MonoBehaviour
{

    //[SerializeField] public GameObject drawer;

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Parent");
        if (col.CompareTag("InteractiveObject"))
        {
            col.transform.parent = this.transform;
            col.attachedRigidbody.useGravity = false;
        }

        if (col.CompareTag("DirectGlow"))
        {
            col.transform.parent = this.transform;
            col.attachedRigidbody.useGravity = false;
        }

    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("Orphan");
        if (col.CompareTag("InteractiveObject"))
        {
            col.attachedRigidbody.useGravity = true;
            col.transform.parent = null;
        }

        if (col.CompareTag("DirectGlow"))
        {
            col.attachedRigidbody.useGravity = true;
            col.transform.parent = null;
        }
    }
}
