using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public string audioClipName;

    public ItemData Hammer;
    public Inventory inventoryScript;

    InteractRaycasting raycast;

    private void Start()
    {
        inventoryScript = FindObjectOfType<Inventory>();
        raycast = FindObjectOfType<InteractRaycasting>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            destroyObject();
        }
    }

    void destroyObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (raycast.raycastInteract(out hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                if (inventoryScript.itemInventory[inventoryScript.selectedItem] == Hammer)
                {
                    SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
                    Destroy(gameObject);
                }
            }            
        }
    }  
}