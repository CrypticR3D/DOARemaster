using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ConnectInputsAndOutputs : MonoBehaviour
{
    public Camera mainCamera;
    public Camera lockCamera;

    private bool interacting = false;

    public GameObject ElectricalBox;
    public GameObject ElectricalBoxInsideMesh;

    InteractRaycasting raycast;
    private bool Disable_Int;
    InventoryMenuToggle inventoryMenuToggle;
    private FirstPersonController PlayerCharacter;

    UIManager uIManager;

    private LineRenderer lineRenderer;
    private Transform currentObject;

    private void Start()
    {
        // Disable the lock camera at the start
        lockCamera.gameObject.SetActive(false);
        raycast = FindObjectOfType<InteractRaycasting>();
        inventoryMenuToggle = FindObjectOfType<InventoryMenuToggle>();
        PlayerCharacter = FindObjectOfType<FirstPersonController>();
        Disable_Int = false;

        uIManager = FindObjectOfType<UIManager>();

    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!Disable_Int)
            {
                if (!interacting)
                {
                    ActivateObject();
                }
            }
        }

        // Check for backing away
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (interacting)
            {
                if (!uIManager.isPaused)
                {
                    StopInteracting();
                }
                
            }
        }
    }
    void ActivateObject()
    {
        RaycastHit hit;
        if (raycast.raycastInteract(out hit))
        {
            if (hit.transform.gameObject == ElectricalBox)
            {
                if (interacting)
                {
                    // Check for combination input
                    checkPuzzleComplete();
                }
                else
                {
                    // Check for starting interaction
                    StartInteracting();
                }
            }
        }
    }

    private void StartInteracting()
    {
        PlayerCharacter.canMove = false;
        PlayerCharacter.canLook = false;
        PlayerCharacter.canCrouch = false;
        inventoryMenuToggle.canOpen = false;

        ElectricalBox.GetComponent<Collider>().enabled = false;
        ElectricalBoxInsideMesh.GetComponent<Collider>().enabled = false;

        // Enable the lock camera and disable the main camera
        mainCamera.gameObject.SetActive(false);
        lockCamera.gameObject.SetActive(true);

        // Enable cursor and lock it to the screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        interacting = true;

        uIManager.isOnPuzzle = true;
    }
    private void StopInteracting()
    {
        PlayerCharacter.canMove = true;
        PlayerCharacter.canLook = true;
        PlayerCharacter.canCrouch = true;
        inventoryMenuToggle.canOpen = true;
        ElectricalBox.GetComponent<Collider>().enabled = true;
        ElectricalBoxInsideMesh.GetComponent<Collider>().enabled = true;

        // Disable the lock camera and enable the main camera
        lockCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Disable cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        interacting = false;

        uIManager.isOnPuzzle = false;
    }
    public bool checkPuzzleComplete()
    {
        return false;
    }
    public void TurnOffLights()
    {

    }
    public bool CheckCombination()
    {
        return false;
    }
}
