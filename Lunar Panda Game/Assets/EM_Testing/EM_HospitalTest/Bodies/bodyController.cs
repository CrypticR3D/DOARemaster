using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{
    public string audioClipName;

    [Header("Puzzle Values")]
    [Tooltip("Whether this patient is the one with the screwdriver or not")]
    public bool isCorrect;

    [Tooltip("The mesh of the body when it has been cut")]
    public GameObject openBody;
    public GameObject closedBody;

    private bool isCut;

    [Header("Item Data")]
    [Tooltip("Item data for the scalpel")]
    public ItemData scalpelData;
    [Tooltip("Screwdriver game object (only needed for the body with the screwdriver)")]
    public GameObject screwdriverTip;
    private bool collected = false;

    private Inventory inventoryScript;

    InteractRaycasting raycast;
    private bool Disable_Int;

    // Start is called before the first frame update
    void Start()
    {
        inventoryScript = FindObjectOfType<Inventory>();
        raycast = FindObjectOfType<InteractRaycasting>();
        isCut = false;
        Disable_Int = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!Disable_Int)
            {
                ActivateObject();
            }
        }
    }

    void ActivateObject()
    {
        RaycastHit hit;

        if (raycast.raycastInteract(out hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                OpenBody();
            }

        }
    }

    public void OpenBody()
    {
        if (inventoryScript.itemInventory[inventoryScript.selectedItem] != null)
        {
            if (inventoryScript.itemInventory[inventoryScript.selectedItem] == scalpelData)
            {
                if (!isCut)
                {
                    SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
                }
                
                changeBody();
                isCut = true;

                if (isCorrect == true)
                {
                    if (collected == false)
                    {
                        screwdriverTip.SetActive(true);
                        collected = true;
                        Disable_Int = true;
                    }
                }

            }
        }
    }

    public void changeBody()
    {
        openBody.SetActive(true);
        closedBody.SetActive(false);
    }
}
