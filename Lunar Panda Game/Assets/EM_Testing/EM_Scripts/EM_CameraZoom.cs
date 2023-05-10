using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class EM_CameraZoom : MonoBehaviour
{
    public float zoomAmount = 2.0f; // Amount to zoom in
    public float zoomSpeed = 5.0f; // Speed of zoom

    private Camera mainCamera; // Reference to main camera
    private float originalFOV; // Original field of view
    private bool isZoomedIn; // Flag for whether the camera is currently zoomed in
    private Vector3 targetPosition; // Target position for the camera to focus on

    private GameObject player;
    private CharacterController CharacterController;
    private FirstPersonController playerFPSController;

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to main camera
        originalFOV = mainCamera.fieldOfView; // Store original field of view
        isZoomedIn = false; // Camera starts out not zoomed in
        player = GameObject.FindWithTag("Player");
        CharacterController = player.GetComponentInChildren<CharacterController>();
        playerFPSController = player.GetComponentInChildren<FirstPersonController>();
    }

    private void Update()
    {
        // Check if the player clicked on an object
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ZoomObject")) // Check if object is a ZoomObject
                {
                    targetPosition = hit.collider.transform.position; // Set target position to center of object
                    StartCoroutine(ZoomIn()); // Start zooming in
                }
            }
        }

        // Check if the player is trying to cancel the zoom
        if (Input.GetMouseButtonDown(1) || Vector3.Distance(transform.position, targetPosition) > 5f)
        {
            if (isZoomedIn) // Only zoom out if currently zoomed in
            {
                StartCoroutine(ZoomOut()); // Start zooming out
            }
        }

        // Check if camera is zoomed in and disable player movement
        if (isZoomedIn)
        {
            player.GetComponentInChildren<FirstPersonController>().enabled = false;
            player.GetComponentInChildren<FirstPersonController>().canLook = false;
        }
    }

    private IEnumerator ZoomIn()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor in center of screen
        Cursor.visible = false; // Hide cursor
        isZoomedIn = true; // Set flag to true
        while (mainCamera.fieldOfView > originalFOV / zoomAmount)
        {
            mainCamera.fieldOfView -= Time.deltaTime * zoomSpeed; // Zoom in gradually
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * zoomSpeed); // Move camera towards center of object
            yield return null;

            // Check if the player is trying to cancel the zoom
            if (Input.GetMouseButtonDown(1) || Vector3.Distance(transform.position, targetPosition) > 5f)
            {
                StartCoroutine(ZoomOut()); // Start zooming out
                yield break; // Exit coroutine early
            }
        }

        // Lock player movement while zoomed in
        player.GetComponentInChildren<FirstPersonController>().enabled = false;
        player.GetComponentInChildren<FirstPersonController>().canLook = false;
        Cursor.visible = true; // Show cursor once zoomed in
    }

    private IEnumerator ZoomOut()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        isZoomedIn = false; // Set flag to false
        // Unlock player movement when zooming out
        player.GetComponentInChildren<FirstPersonController>().enabled = true;
        player.GetComponentInChildren<FirstPersonController>().canLook = true;
        while (mainCamera.fieldOfView < originalFOV)
        {
            mainCamera.fieldOfView += Time.deltaTime * zoomSpeed; // Zoom out gradually
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZoomTrigger")) // Check if object is a ZoomTrigger
        {
            StartCoroutine(ZoomOut()); // Start zooming out
        }
    }
}