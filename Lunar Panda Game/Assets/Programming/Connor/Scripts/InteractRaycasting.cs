using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRaycasting : MonoBehaviour
{
    Transform player;
    //Flashlight flashlight;
    Transform playerCamera;
    JournalMenuToggle Journal;
    PauseButtonToggle Pause;
    FeedbackToggle Feedback;
    InventoryMenuToggle Inventory;
    private static InteractRaycasting _instance;
    public static InteractRaycasting Instance { get { return _instance; } }


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main.transform;
        //flashlight = FindObjectOfType<Flashlight>();
        //flashlight.enabled = false;

        //setting up singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        //Pause = FindObjectOfType<PauseButtonToggle>();
        Journal = FindObjectOfType<JournalMenuToggle>();
        Feedback = FindObjectOfType<FeedbackToggle>();
        Inventory = FindObjectOfType<InventoryMenuToggle>();
        Pause = FindObjectOfType<PauseButtonToggle>();
    }

    public bool raycastInteract(out RaycastHit hit)
    {
        if(Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, player.GetComponent<PlayerPickup>().pickupDist))
        {
            if (hit.transform != null && !Journal.IsOnMenu && !Feedback.IsOnFeedbackMenu && !Inventory.IsOnInventory && !Pause.IsPaused)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public bool raycastInteractLayer(out RaycastHit hit, int layerMask)
    {
        if(Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, player.GetComponent<PlayerPickup>().pickupDist, layerMask))
        {
            if (hit.transform != null && !Journal.IsOnMenu && !Feedback.IsOnFeedbackMenu && !Inventory.IsOnInventory && !Pause.IsPaused)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
