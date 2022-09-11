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
    private bool inRange = false;

    [Header("Game Objects")]
    [Tooltip("Drag the tooltip text in the scene here")]
    public GameObject UITip;
    [Tooltip("Drag in the journalMenu object if it is a journal-related tooltip")]
    public GameObject journalMenu;
    [Tooltip("Drag in the InventoryMenu object if it is an inventory-related tooltip")]
    public GameObject inventoryMenu;

    public GameObject FLTriggerBox;
    public GameObject torch;
    public GameObject TooltipTrigger;

    public GameObject FlashlightPrompt;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkControls();
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
                        if (Input.GetButton("Flashlight"))
                        {
                            deactivateTooltip();
                            FLTriggerBox.SetActive(false);
                        }

                    }
                    break;
                case controlTypes.SPRINT:
                    {
                        if (Input.GetButton("Sprint"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.OPENINV:
                    {
                        if (Input.GetButton("Inventory"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.CLOSEINV:
                    {
                        //If Inventory is currently up
                        if (Input.GetButton("Inventory"))
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
                        if (Input.GetButton("Journal"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.CLOSEJRN:
                    {
                        if (Input.GetButton("Journal"))
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
                        if (Input.GetKey("e"))
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
                        if (Input.GetKey("PutAway"))
                        {
                                deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.INTERACT:
                    {
                        if (Input.GetKey("mouse0"))
                        {
                            deactivateTooltip();
                        }
                    }
                    break;
                case controlTypes.PROMPTENABLE:
                    {
                        FlashlightPrompt.SetActive(true);
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
        if (col.tag == "TipCollider" || col.tag == "Player")
        {
            inRange = true;
            UITip.SetActive(true);
            UITip.GetComponent<tooltipDisplay>().changeText(tooltipMessage);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "TipCollider" || col.tag == "Player")
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
