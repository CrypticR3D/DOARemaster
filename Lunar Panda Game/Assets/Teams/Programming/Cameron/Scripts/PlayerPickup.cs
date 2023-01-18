using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPickup : MonoBehaviour
{
    // Transform of the main camera
    Transform playerCameraTransform;
    Transform player;

    public float pickupDist = 3f;
    public GameObject heldItem;

    [SerializeField] public GameObject IsLookingAt = null;
    internal bool holdingNarrative = false;

    [Tooltip("The layers that the raycasts will ignore")]
    [SerializeField] LayerMask rayMask;

    // Reference to the inventory script
    Inventory inventory;

    // Reference to the player's movement script
    playerMovement pMovement;

    // Reference to the raycasting script on the player
    InteractRaycasting playerPickupRay;

    void Awake()
    {
        playerCameraTransform = Camera.main.transform;
        inventory = FindObjectOfType<Inventory>();
        pMovement = FindObjectOfType<playerMovement>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPickupRay = player.GetComponent<InteractRaycasting>();
    }

    void Update()
    {
        if (heldItem != null)
        {
            if (heldItem.GetComponent<Rigidbody>() == null)
            {
                heldItem.AddComponent<Rigidbody>();
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            if (playerPickupRay.raycastInteract(out hit))
            {
                if (hit.transform.GetComponent<HoldableItem>())
                {
                    // Add the item's data to the player's inventory
                    if (hit.transform.GetComponent<HoldableItem>().data)
                        inventory.addItem(hit.transform.GetComponent<HoldableItem>().data);
                        PickupItem(hit.transform);

                    // Destroy the item gameobject
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }
    }

    internal void PickupItem(Transform item)
    {
        heldItem = item.gameObject;
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

//public class PlayerPickup : MonoBehaviour
//{
//    Transform playerCameraTransform;
//    [Tooltip("The layers that the raycasts will ignore")]
//    [SerializeField] LayerMask rayMask;

//    [Header("Pickup/Hold System")]
//    public GameObject heldItem;
//    Vector3 mouseRotateStartPoint;
//    Quaternion itemStartRotation;
//    public float pickupDist = 3f;
//    float startingHoldDist;
//    public float holdDist = 1.5f;
//    [SerializeField] float dropDist;
//    [SerializeField] float lerpSpeed;
//    [Range(100f, 500f)]
//    [SerializeField] float ControllerItemRotateSens;

//    [Header("Lookat System")]
//    [SerializeField] public GameObject GOLookingAt = null;


//    Inventory inventory;
//    bool controllerRotating = false;

//    [Header("Rotatable Items")]
//    [Tooltip("The distance away from the camera that rotatable items are put")]
//    [SerializeField] float rotDist;
//    internal bool holdingNarrative = false;
//    playerMovement pMovement;

//    Transform player;
//    InteractRaycasting playerPickupRay;

//    public FirstPersonController PlayerCharacter;

//    void Awake()
//    {
//        PlayerCharacter = FindObjectOfType<FirstPersonController>();
//        playerCameraTransform = Camera.main.transform;
//        inventory = FindObjectOfType<Inventory>();

//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        playerPickupRay = player.GetComponent<InteractRaycasting>();
//        startingHoldDist = holdDist;
//        pMovement = FindObjectOfType<playerMovement>();


//    }

//    void Update()
//    {
//        if (heldItem != null)
//        {
//            if (heldItem.GetComponent<Rigidbody>() == null)
//            {
//                heldItem.AddComponent<Rigidbody>();
//            }
//        }

//        if (Input.GetButtonDown("Interact") && heldItem == null)
//        {

//            RaycastHit hit;
//            if (playerPickupRay.raycastInteract(out hit))
//            {
//                if (hit.transform.GetComponent<HoldableItem>())
//                {

//                    holdDist = startingHoldDist;

//                    if (hit.transform.GetComponent<HoldableItem>().data)
//                        inventory.addItem(hit.transform.GetComponent<HoldableItem>().data);
//                    PickupItem(hit.transform);

//                    if (GOLookingAt != null && GOLookingAt.GetComponent<GlowWhenLookedAt>() != null)
//                        if (GOLookingAt.GetComponent<GlowWhenLookedAt>().isGlowing)
//                            GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();
//                    GOLookingAt = null;
//                }

//            }
//        }
//        else if (Input.GetButtonDown("Interact") && heldItem != null)
//        {

//            {
//                DropHeldItem();
//            }
//        }

//        if (Input.GetButtonDown("Throw") && heldItem != null)
//        {
//            if (!heldItem.GetComponent<RotatableItem>())
//            {
//                ThrowItem();
//            }
//        }
//        RotateHeldItem();
//        CheckInfront();
//    }

//    void CheckInfront()
//    {
//        RaycastHit hit;

//        if (playerPickupRay.raycastInteract(out hit))
//        {

//            if (hit.transform.GetComponent<GlowWhenLookedAt>() && heldItem == null)
//            {
//                if (GOLookingAt && GOLookingAt != hit.transform.gameObject)
//                {
//                    GOLookingAt.GetComponent<GlowWhenLookedAt>().isGlowing = false;

//                    GOLookingAt = null;
//                }

//                if (!hit.transform.GetComponent<GlowWhenLookedAt>().isGlowing)
//                {
//                    GOLookingAt = hit.transform.gameObject;
//                    GOLookingAt.GetComponent<GlowWhenLookedAt>().ToggleGlowingMat();

//                    if (GOLookingAt.GetComponent<tooltipController>() != null)
//                    {
//                        GOLookingAt.GetComponent<tooltipController>().EnableTooltip();
//                    }

//                    else
//                    {

//                    }


//                }
//            }
//            else if (GOLookingAt)
//            {
//                GOLookingAt.GetComponent<GlowWhenLookedAt>().isGlowing = false;

//                GOLookingAt = null;
//            }
//        }
//        else if (GOLookingAt)
//        {
//            GOLookingAt.GetComponent<GlowWhenLookedAt>().isGlowing = false;

//            GOLookingAt = null;
//        }

//    }

//    void RotateHeldItem()
//    {

//        if (Input.GetButtonDown("RotateItemEnable") && heldItem != null)
//        {
//            mouseRotateStartPoint = Input.mousePosition;
//            heldItem.transform.localRotation = Quaternion.identity;
//            heldItem.transform.eulerAngles = new Vector3(heldItem.transform.localEulerAngles.x, heldItem.transform.localEulerAngles.y + transform.localEulerAngles.y, heldItem.transform.localEulerAngles.z);
//            itemStartRotation = heldItem.transform.rotation;
//            pMovement.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
//            pMovement.enabled = false;
//            if(!FindObjectOfType<PauseButtonToggle>().IsPaused)
//            {
//                Cursor.lockState = CursorLockMode.None;
//                playerCameraTransform.GetComponent<lockMouse>().canLook = false;
//            }

//        }

//        else if (Input.GetButton("RotateItemEnable") && heldItem != null)
//        {

//            Vector2 distBetweenStartPoint = new Vector2((Input.mousePosition - mouseRotateStartPoint).x, (Input.mousePosition - mouseRotateStartPoint).y);

//            heldItem.transform.rotation = itemStartRotation * Quaternion.Euler(new Vector3((distBetweenStartPoint.y / Screen.width) * 360, 0, (distBetweenStartPoint.x / Screen.width) * -360));
//        }

//        if (Input.GetButtonUp("RotateItemEnable"))
//        {

//            if (!heldItem.GetComponent<RotatableItem>())
//            {
//                Cursor.lockState = CursorLockMode.Locked;
//                playerCameraTransform.GetComponent<lockMouse>().canLook = true;

//            }

//            if (heldItem)
//            {
//                heldItem.transform.localRotation = Quaternion.identity;
//                heldItem.transform.eulerAngles = new Vector3(heldItem.transform.localEulerAngles.x, heldItem.transform.localEulerAngles.y + transform.localEulerAngles.y, heldItem.transform.localEulerAngles.z);
//                pMovement.enabled = true;
//            }
//        }

//        if ((Input.GetAxisRaw("ItemRotateX") != 0 || Input.GetAxisRaw("ItemRotateY") != 0) && heldItem != null)
//        {
//            if (controllerRotating) 
//            {
//                heldItem.transform.RotateAround(heldItem.transform.position, heldItem.transform.up, Input.GetAxisRaw("ItemRotateX") * ControllerItemRotateSens * Time.deltaTime);
//                heldItem.transform.RotateAround(heldItem.transform.position, heldItem.transform.right, Input.GetAxisRaw("ItemRotateY") * ControllerItemRotateSens * Time.deltaTime);

//            }
//            else
//            {

//                controllerRotating = true;
//            }
//        }
//        else if (Input.GetAxisRaw("ItemRotateX") == 0 && Input.GetAxisRaw("ItemRotateY") == 0)
//        {
//            controllerRotating = false;
//        }
//    }


//    void ThrowItem()
//    {
//        Rigidbody heldItemRB = heldItem.GetComponent<Rigidbody>();
//        heldItemRB.AddForce(playerCameraTransform.forward * throwForce, ForceMode.Impulse);
//        DropHeldItem();
//    }

//    void FixedUpdate()
//    {
//        if (heldItem != null)
//        {

//            Vector3 direction = (playerCameraTransform.position + (playerCameraTransform.forward * holdDist)) - heldItem.transform.position;


//            float dist = Vector3.Distance(heldItem.transform.position, playerCameraTransform.position + (playerCameraTransform.forward * holdDist));


//            if (dist > dropDist)
//            {
//                DropHeldItem();
//                return;
//            }

//            if(heldItem.GetComponent<Rigidbody>())
//                heldItem.GetComponent<Rigidbody>().velocity = direction.normalized * lerpSpeed * dist;
//        }
//    }

//    public void DropHeldItem()
//    {
//        inventory.removeItem();


//        heldItem.GetComponent<Rigidbody>().useGravity = true;
//        heldItem.GetComponent<Rigidbody>().freezeRotation = false;


//        heldItem = null;
//    }

//    internal void PickupItem(Transform item)
//    {


//        item.GetComponent<Rigidbody>().isKinematic = false;
//        item.GetComponent<Rigidbody>().useGravity = false;
//        item.GetComponent<Rigidbody>().freezeRotation = true;
//        heldItem = item.gameObject;
//    }
//}