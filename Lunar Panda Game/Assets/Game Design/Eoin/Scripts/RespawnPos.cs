using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPos : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Respawner")
        {
            transform.position = startPos;
        }
    }
}
