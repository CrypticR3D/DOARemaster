/*
Quick^tm note from Cam:
All holdable items should have the "HoldableItem" script on it
The player should also have the "Player" tag on it (in the inspector)
I tested this in my own scene and it worked well. Due to the lack of information on what the designers want, i made it so that if the held item
hits an object, the player drops it. Can change this in the future if we want.
I was also thinking that we should maybe make an "Interact" script that any script can call and it would find the item infront of it (instead of
having one in a bunch of different scripts)

With the rotation, I will have to integrate it with the character looking code but it works pretty well. I could also change the keybindings if needed
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    Transform playerCameraTransform;
    [Tooltip("The layers that the raycasts will ignore")]
    [SerializeField] LayerMask rayMask;
    [Header("Pickup/Hold System")]
    public GameObject heldItem;
    Vector3 mouseRotateStartPoint;
    Quaternion itemStartRotation;
    public float pickupDist = 3f;
    public float holdDist = 1.5f;
    [SerializeField] float lerpAmnt;
    public float lerpSpeed;
    [HideInInspector] public float slowLerpSpeed;
    [HideInInspector] public float currentLerpSpeed;
    [Header("Lookat System")]
    [SerializeField] GameObject GOLookingAt = null;
    [Header("Throw System")]
    [SerializeField] float throwForce;
    Inventory inventory;

    void Awake()
    {
        playerCameraTransform = Camera.main.transform;
        inventory = FindObjectOfType<Inventory>();
        slowLerpSpeed = lerpSpeed / 4;
        currentLerpSpeed = lerpSpeed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && heldItem == null)
        {
            RaycastHit hit;
            //Casts a ray from the camera
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.TransformDirection(Vector3.forward), out hit, pickupDist))
            {
                if (hit.transform.GetComponent<HoldableItem>())
                {
                    //if the ray hits a holdable item, the player picks it up
                    inventory.addItem(hit.transform.GetComponent<HoldableItem>().data);
                    PickupItem(hit.transform);
                    //if the player is looking at an object that can glow, and if the object is currently glowing, toggle the glow effect
                    if (GOLookingAt != null && GOLookingAt.GetComponent<GlowWhenLookedAt>() != null)
                        if(GOLookingAt.GetComponent<GlowWhenLookedAt>().isGlowing)
                            GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
                    GOLookingAt = null;
                }                
            }
        }
        else if(Input.GetButtonDown("Interact") && heldItem != null)
        {
            //if the player is holding an item and presses 'e', it drops said item
            
            DropHeldItem();
        }
        if (Input.GetButtonDown("Throw") && heldItem != null)
        {
            ThrowItem();
        }
        RotateHeldItem();
        CheckInfront();
    }

    void CheckInfront()
    {
        RaycastHit hit;
        //Casts a ray from the camera
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.TransformDirection(Vector3.forward), out hit, pickupDist))
        {
            //all these if's and elses are to handle all edge cases so that when you stop looking at a glowable object, it stops glowing properly
            if (hit.transform.GetComponent<GlowWhenLookedAt>() && heldItem == null)
            {
                print("Working?");
                if (GOLookingAt && GOLookingAt != hit.transform.gameObject)
                {

                    GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
                    GOLookingAt = null;
                }
                if (!hit.transform.GetComponent<GlowWhenLookedAt>().isGlowing)
                {
                    GOLookingAt = hit.transform.gameObject;
                    GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
                }
            }
            else if (GOLookingAt)
            {
                GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
                GOLookingAt = null;
            }
        }
        else if (GOLookingAt)
        { 
            GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
            GOLookingAt = null;
        }
    }

    void RotateHeldItem()
    {
        //the frame the player clicks and is holding an item
        if (Input.GetButtonDown("Fire1") && heldItem != null)
        {
            //setup/start the rotation mode
            mouseRotateStartPoint = Input.mousePosition;
            heldItem.transform.localRotation = Quaternion.identity;
            heldItem.transform.eulerAngles = new Vector3(heldItem.transform.localEulerAngles.x, heldItem.transform.localEulerAngles.y + transform.localEulerAngles.y, heldItem.transform.localEulerAngles.z);
            itemStartRotation = heldItem.transform.rotation;
            Cursor.lockState = CursorLockMode.None;
            playerCameraTransform.GetComponent<lockMouse>().canLook = false;
        }
        //while the mouse button is down and player is holding an item
        else if (Input.GetButton("Fire1") && heldItem != null)
        {
            //get the distance between the first clicks mouse position, and the current mouse position
            Vector2 distBetweenStartPoint = new Vector2((Input.mousePosition - mouseRotateStartPoint).x, (Input.mousePosition - mouseRotateStartPoint).y);
            //rotate the held item based on the distance between the start mouse pos and the current mouse pos
            heldItem.transform.rotation = itemStartRotation * Quaternion.Euler(new Vector3((distBetweenStartPoint.y / Screen.width) * 360, 0, (distBetweenStartPoint.x / Screen.width) * -360));
        }
        //the frame the player stops pressing the mouse button
        if (Input.GetButtonUp("Fire1"))
        {
            //stop rotating the object
            Cursor.lockState = CursorLockMode.Locked;
            playerCameraTransform.GetComponent<lockMouse>().canLook = true;
            if (heldItem)
            {
                heldItem.transform.localRotation = Quaternion.identity;
                heldItem.transform.eulerAngles = new Vector3(heldItem.transform.localEulerAngles.x, heldItem.transform.localEulerAngles.y + transform.localEulerAngles.y, heldItem.transform.localEulerAngles.z);
            }


        }
    }

    //yeets held object using the throwForce variable that the designers can balance
    void ThrowItem()
    {
        
        Rigidbody heldItemRB = heldItem.GetComponent<Rigidbody>();
        heldItemRB.AddForce(playerCameraTransform.forward * throwForce, ForceMode.Impulse);
        DropHeldItem();
    }

    void LateUpdate()
    {
        if (heldItem != null)
        {
            Vector3 vel = new Vector3(heldItem.GetComponent<Rigidbody>().velocity.x, heldItem.GetComponent<Rigidbody>().velocity.y, heldItem.GetComponent<Rigidbody>().velocity.z);
            heldItem.GetComponent<Rigidbody>().position = Vector3.SmoothDamp(heldItem.transform.position, playerCameraTransform.position + playerCameraTransform.forward * holdDist, ref vel, Time.deltaTime * lerpAmnt, currentLerpSpeed);
        }
    }

    public void DropHeldItem()
    {
        inventory.removeItem();
        //heldItem.transform.parent = null;
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().freezeRotation = false;
        heldItem = null;
    }

    internal void PickupItem(Transform item)
    {
        //item.parent = playerCameraTransform;
        //item.localPosition = new Vector3(0, 0, holdDist);
        //the below code is needed so that the rotation is user-friendly and feels more natural to the player
        item.transform.rotation = Quaternion.identity;
        item.transform.eulerAngles = new Vector3(item.transform.eulerAngles.x, transform.eulerAngles.y, item.transform.eulerAngles.z);
        
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().freezeRotation = true;
        heldItem = item.gameObject;
    }
}
