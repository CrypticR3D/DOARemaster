using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent2Drawer : MonoBehaviour
{

    [SerializeField] public GameObject drawer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Parent");
        other.transform.parent = drawer.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Orphan");
        other.transform.parent = null;
    }
}
