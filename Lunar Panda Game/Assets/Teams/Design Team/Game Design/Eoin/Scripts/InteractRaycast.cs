using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KeySystem;

public class InteractRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private MyDoorController raycastedDoor;
    private MyDrawerController raycastedDrawer;
    private KeyItemController raycastedKey;

    private Destructable raycastedWood;

    private Disc raycastedDisk;

    //private MyNewController raycastedObj;

    [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;

    [SerializeField] private Image crosshair = null;
    private bool isCrosshairActive;
    private bool doOnce;

    private const string DoorTag = "Door";

    private const string DrawerTag = "Drawer";

    private const string KeyTag = "Key";

    private const string InteractTag = "InteractiveObject";

    private const string GlowTag = "DirectGlow";

    private const string TriggerTag = "Trigger";

    Transform player;
    InteractRaycasting playerPickupRay;

    // Transform player;
    //Flashlight flashlight;
    //Transform playerCamera;
    JournalMenuToggle Journal;
    PauseButtonToggle Pause;
    FeedbackToggle Feedback;
    InventoryMenuToggle Inventory;
    //private static InteractRaycasting _instance;
    //public static InteractRaycasting Instance { get { return _instance; } }

    public Transform playerCamera;
    [SerializeField] public Inventory Inv = null;
    [SerializeField] ItemData Hammer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPickupRay = player.GetComponent<InteractRaycasting>();
        playerCamera = Camera.main.transform;
        Inv = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        //Pause = FindObjectOfType<PauseButtonToggle>();
        Journal = FindObjectOfType<JournalMenuToggle>();
        Feedback = FindObjectOfType<FeedbackToggle>();
        Inventory = FindObjectOfType<InventoryMenuToggle>();
        Pause = FindObjectOfType<PauseButtonToggle>();
    }



    private void Update()
    {
        // RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd, Color.green);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        //if (InteractRaycasting.Instance.raycastInteractLayer(out RaycastHit hit, mask)) //(Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        if (Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out RaycastHit hit, player.GetComponent<PlayerPickup>().pickupDist, mask))
        {
            if (hit.transform != null && !Journal.IsOnMenu && !Feedback.IsOnFeedbackMenu && !Inventory.IsOnInventory && !Pause.IsPaused)
            {

                //if (hit.collider.GetComponent<Destructable>() && Inv.itemInventory[Inv.selectedItem] == Hammer)
                //{
                //    if (!doOnce)
                //    {
                //        raycastedWood = hit.collider.gameObject.GetComponent<Destructable>();
                //        CrosshairChange(true);
                //    }

                //    isCrosshairActive = true;
                //    doOnce = true;

                //    if (Input.GetKeyDown(openDoorKey))
                //    {
                //        //Debug.Log("ClickDoor");
                //        Debug.Log("There");
                //        raycastedWood.destroyObject();
                //    }
               // }

                print(hit.transform.name);

                if (hit.collider.CompareTag(TriggerTag))
                {
                    //CrosshairChange(false);
                    //doOnce = false;
                   // Debug.Log("TriggerHit");
                }

                if (hit.collider.CompareTag(GlowTag))
                {
                    if (!doOnce)
                    {
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;
                }

                ///Door Interaction///
                if (hit.collider.CompareTag(DoorTag))
                {
                    if (!doOnce)
                    {
                        raycastedDoor = hit.collider.gameObject.GetComponent<MyDoorController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickDoor");
                        raycastedDoor.PlayAnimation();
                    }
                }

                ///Drawer Interaction///
                if (hit.collider.CompareTag(DrawerTag))
                {


                    if (hit.collider.gameObject.GetComponent<MyDrawerController>())
                    {
                        raycastedDrawer = hit.collider.gameObject.GetComponent<MyDrawerController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (hit.collider.gameObject.GetComponentInParent<MyDrawerController>())
                    {
                        raycastedDrawer = hit.collider.gameObject.GetComponentInParent<MyDrawerController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickDrawer");
                        raycastedDrawer.PlayAnimation();
                    }
                }

                ///Key Interaction///
                if (hit.collider.CompareTag(KeyTag))
                {
                    //Debug.Log("Yikes");
                    if (!doOnce)
                    {
                        raycastedKey = hit.collider.gameObject.GetComponent<KeyItemController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickKey");
                        raycastedKey.ObjectInteraction();
                    }
                }
                if (hit.collider.CompareTag(InteractTag))
                {
                    //Debug.Log("Yikes");
                    if (!doOnce)
                    {
                        raycastedDisk = hit.collider.gameObject.GetComponent<Disc>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetButtonDown("Interact"))
                    {
                        if (hit.transform.gameObject == raycastedDisk)
                        {
                            raycastedDisk.Interact();
                        }

                        //Debug.Log("ClickKey");

                    }
                }
            }
        }

        else
        {
            if (isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
    }

    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
        {
            crosshair.color = Color.red;
        }

        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }
}