using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPressurePlate : MonoBehaviour
{
    [Tooltip("Enter Name of Book in inspector")]
    public string bookName;

    private float weightNeeded;
    private float bookWeight;
    public GameObject evilBook;

    void Start()
    {
        weightNeeded = GetComponent<BookPuzzle>().weightNeeded;
    }

    void Update()
    {
        bookWeight = evilBook.transform.localScale.x * 10;
        Debug.Log(bookWeight);
    }

    private void OnTriggerEnter(Collider pog)
    {
        if (pog.gameObject.name == "EvilBook" && bookWeight > weightNeeded)
        {
            Debug.Log("door would open here");
        }
    }

    private void OnTriggerExit(Collider pog)
    {
 
        if (pog.gameObject.name == "EvilBook" && bookWeight > weightNeeded)
        {
            Debug.Log("door would shut here");
        }
    }
}
