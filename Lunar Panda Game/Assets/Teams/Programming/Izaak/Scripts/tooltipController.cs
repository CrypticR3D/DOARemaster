using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tooltipController : MonoBehaviour
{
    public enum controlTypes
    {
        NONE, MOVE, JUMP, SPRINT, CROUCH, FLASHLIGHT,
        OPENINV, CLOSEINV, OPENJRN, CLOSEJRN, TURNPAGEJRN,
        ITEMSTORE, INTERACT, PROMPTENABLE
    }
    [Header("Controls")]
    [Tooltip("The action you must complete to get rid of the tooltip")]
    public controlTypes input;

    [Header("Parameters")]
    [Tooltip("The message that pops up in the tooltip")]
    public string tooltipMessage;

    [Tooltip("The range the player must be in to activate the tooltip (set to the same as the sphere collider radius)")]
    public float tooltipRange;
    public bool inRange = false;
    public static bool FlashlightEnabled = false;

    [Header("Game Objects")]
    [Tooltip("Drag the tooltip text in the scene here")]
    public GameObject UITip;
    [Tooltip("Drag in the journalMenu object if it is a journal-related tooltip")]
    public GameObject journalMenu;
    [Tooltip("Drag in the InventoryMenu object if it is an inventory-related tooltip")]
    public GameObject inventoryMenu;

    //public GameObject FLTriggerBox;
    //public GameObject torch;
    public GameObject TooltipTrigger;

    public GameObject EnableUI;

    PlayerPickup playerPickup;

    private void Start()
    {
        playerPickup = FindObjectOfType<PlayerPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        checkControls();

        if (playerPickup.GOLookingAt != null)
        {
            inRange = true;
        }
        
    }

    void checkControls()
    {
        if (inRange)
        {
            Debug.Log(inRange);
            switch (input)
            {
                case controlTypes.MOVE:
                    {
                        if ((Input.GetButton("Horizontal")) || (Input.GetButton("Vertical")))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.JUMP:
                    {
                        if (Input.GetButton("Jump"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.CROUCH:
                    {
                        if (Input.GetButton("Crouch"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.FLASHLIGHT:
                    {
                        if (Input.GetButtonDown("Flashlight"))
                        {
                            deactivateTooltip();
                            EnableUI.SetActive(true);
                            FlashlightEnabled = true;

                            //FLTriggerBox.SetActive(false);
                        }

                    }
                    break;
                case controlTypes.SPRINT:
                    {
                        if (Input.GetButtonDown("Sprint"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.OPENINV:
                    {
                        if (Input.GetButtonDown("Inventory"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.CLOSEINV:
                    {
                        //If Inventory is currently up
                        if (Input.GetButtonDown("Inventory"))
                        {
                            if (inventoryMenu.GetComponent<InventoryMenuToggle>().IsOnInventory)
                            {
                                deactivateTooltip();
                            }
                        }
                    }
                    break;
                case controlTypes.OPENJRN:
                    {
                        if (Input.GetButtonDown("Journal"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.CLOSEJRN:
                    {
                        if (Input.GetButtonDown("Journal"))
                        {
                            if (journalMenu.GetComponent<JournalMenuToggle>().IsOnMenu)
                            {
                                deactivateTooltip();
                            }
                        }
                    }
                    break;
                case controlTypes.TURNPAGEJRN:
                    {
                        if (Input.GetButtonDown("e"))
                        {
                            if (journalMenu.GetComponent<JournalMenuToggle>().IsOnMenu)
                            {
                                deactivateTooltip();
                            }
                        }
                    }
                    break;
                case controlTypes.ITEMSTORE:
                    {
                        if (Input.GetButtonDown("PutAway"))
                        {
                                deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.INTERACT:
                    {
                        if (Input.GetButtonDown("Interact"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.PROMPTENABLE:
                    {
                        //UIPrompt.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void deactivateTooltip()
    {
        inRange = false;
        UITip.GetComponent<tooltipDisplay>().changeText(" ");
        UITip.SetActive(false);
        GameObject.Destroy(TooltipTrigger);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("TipCollider") || col.CompareTag("Player"))
        {
            inRange = true;
            UITip.SetActive(true);
            UITip.GetComponent<tooltipDisplay>().changeText(tooltipMessage);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("TipCollider") || col.CompareTag("Player"))
        {
            if (input == controlTypes.NONE)
            {
                inRange = false;
                UITip.GetComponent<tooltipDisplay>().changeText(" ");
                UITip.SetActive(false);
                GameObject.Destroy(TooltipTrigger);
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tooltipRange);
    }
}
