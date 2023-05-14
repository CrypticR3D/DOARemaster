using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEndScript : MonoBehaviour
{

    [SerializeField] public GameObject TheEndObject;
    [SerializeField] public GameObject TheFireObject;
    [SerializeField] public GameObject TheLightObject;
    [SerializeField] public GameObject TheClockObject;
    [SerializeField] public GameObject TheOldClockObject;


    Inventory inventory;
    [SerializeField] ItemData FinalKeyData;
    [SerializeField] public bool doOnce;


    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.itemInventory.Contains (FinalKeyData))
        {
            if (doOnce == false)
            {
                EnableTheEnd();
                
                //inventory.selectItem(0);
                //inventory.itemInventory[inventory.selectedItem] == null;
            }
            doOnce = true;
        }
    }

    void EnableTheEnd()
    {
        TheEndObject.SetActive(true);
        TheFireObject.SetActive(true);
        TheLightObject.SetActive(true);
        TheClockObject.SetActive(true);
        TheOldClockObject.SetActive(false);
    }
}
