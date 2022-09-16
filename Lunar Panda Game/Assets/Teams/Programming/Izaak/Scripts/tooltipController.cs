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
    float tooltipTimer;
    bool TipComplete;


    private void Start()
    {
        playerPickup = FindObjectOfType<PlayerPickup>();
        TipComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkControls();

        if (playerPickup.GOLookingAt != null)
        {
            tooltipTimer = 0;
            inRange = true;
        }

        if (UITip.gameObject.activeInHierarchy)
        {
            tooltipTimer += Time.deltaTime;

            if (tooltipTimer >= 3)
            {
                DisableTooltip();
            }
        }

    }

    void checkControls()
    {
        if (inRange)
        {
            //Debug.Log(inRange);
            switch (input)
            {
                case controlTypes.MOVE:
                    {
                        if ((Input.GetButton("Horizontal")) || (Input.GetButton("Vertical")))
                        {
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.JUMP:
                    {
                        if (Input.GetButton("Jump"))
                        {
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.CROUCH:
                    {
                        if (Input.GetButton("Crouch"))
                        {
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.FLASHLIGHT:
                    {
                        if (Input.GetButtonDown("Flashlight"))
                        {
                            DisableTooltip();
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
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.OPENINV:
                    {
                        if (Input.GetButtonDown("Inventory"))
                        {
                            DisableTooltip();
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
                                DisableTooltip();
                            }
                        }
                    }
                    break;
                case controlTypes.OPENJRN:
                    {
                        if (Input.GetButtonDown("Journal"))
                        {
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.CLOSEJRN:
                    {
                        if (Input.GetButtonDown("Journal"))
                        {
                            if (journalMenu.GetComponent<JournalMenuToggle>().IsOnMenu)
                            {
                                DisableTooltip();
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
                                DisableTooltip();
                            }
                        }
                    }
                    break;
                case controlTypes.ITEMSTORE:
                    {
                        if (Input.GetButtonDown("PutAway"))
                        {
                            DisableTooltip();
                        }
                    }
                    break;
                case controlTypes.INTERACT:
                    {
                        if (Input.GetButtonDown("Interact"))
                        {
                            DisableTooltip();
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


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            EnableTooltip();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (input == controlTypes.NONE)
            {
                HideTip();
            }
        }

    }

    public void EnableTooltip()
    {
        if (TipComplete == false)
        {
            gameObject.SetActive(true);
            inRange = true;
            UITip.SetActive(true);
            UITip.GetComponent<tooltipDisplay>().changeText(tooltipMessage);

        }
    }

    public void HideTip()
    {
        //TipComplete = false;
        inRange = false;
        UITip.SetActive(false);

    }

    public void DisableTooltip()
    {
        TipComplete = true;
        inRange = false;
        UITip.GetComponent<tooltipDisplay>().changeText(" ");
        UITip.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tooltipRange);
    }

}