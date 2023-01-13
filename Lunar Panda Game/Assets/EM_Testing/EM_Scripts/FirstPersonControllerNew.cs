using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControllerNew : MonoBehaviour
{

    // Variables for movement speed, mouse sensitivity and gravity
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float crouchSpeed = 2f;
    public float gravity = 9.81f; // new variable for gravity

    // Variables for storing the camera's transform and the character controller component
    private Transform cameraTransform;
    private CharacterController characterController;
    private bool isCrouching = false;
    private Vector3 gravityVector;


    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    public float crouchTimeDown = 0.5F;
    public float crouchTimeUp = 0.1F;


    void Start()
    {
        // Get the camera's transform and the character controller component
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the horizontal and vertical input axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if the player wants to crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        // Calculate the movement vector based on the input axis
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        

        // Rotate the movement vector based on the camera's rotation
        movement = cameraTransform.TransformDirection(movement);

        // Normalize the movement vector
        movement.Normalize();

        // Check if the player is crouching
        if (isCrouching)
        {
            // Multiply the movement vector by the crouch speed
            movement *= crouchSpeed;
            characterController.height = Mathf.MoveTowards(characterController.height, 1.0f, Time.deltaTime / crouchTimeDown);
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, 1, Time.deltaTime / crouchTimeDown);
        }

        else
        {
            // Multiply the movement vector by the movement speed
            movement *= movementSpeed;
            characterController.height = Mathf.MoveTowards(characterController.height, 2.0f, Time.deltaTime / crouchTimeUp);
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, 2, Time.deltaTime / crouchTimeDown);
        }

        // Apply gravity to the player
        gravityVector.y -= gravity * Time.deltaTime;
        movement += gravityVector;

        // Apply the movement vector to the character controller
        characterController.Move(movement * Time.deltaTime);

        // Clamp the player camera
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}