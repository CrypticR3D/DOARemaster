using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PadlockInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public Camera lockCamera;

    private bool interacting = false;

    [Header("Codes")]
    [Tooltip("The combination that solves the puzzle")]
    public int[] correctCode;
    [Tooltip("The combination displayed before the player has interacted with it")]
    public int[] currentCode;

    [Header("Puzzle State")]
    [Tooltip("Check for if the puzzle is solved")]
    private bool puzzleSolved;

    InteractRaycasting raycast;
    private bool Disable_Int;

    public GameObject Padlock;

    InventoryMenuToggle inventoryMenuToggle;

    [SerializeField] InteractAnimation interactAnimation;

    public string audioClipName;

    private FirstPersonController PlayerCharacter;
    UIManager uIManager;

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

        // Check for backing away from the lock
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (interacting)
            {
                StopInteracting();
            }
        }
    }
    void ActivateObject()
    {
        RaycastHit hit;
        if (raycast.raycastInteract(out hit))
        {
            if (hit.transform.gameObject == Padlock)
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
        Padlock.GetComponent<Collider>().enabled = false;

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
        Padlock.GetComponent<Collider>().enabled = true;

        // Disable the lock camera and enable the main camera
        lockCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Disable cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        interacting = false;

        uIManager.isOnPuzzle = false;
    }

    public int getCurrentCode(int digitPlace)
    {
        return currentCode[digitPlace - 1];
    }

    public void changeCurrentCode(int placement, int value)
    {
        currentCode[placement - 1] = value;
    }

    public bool checkPuzzleComplete()
    {
        int passes = 0;
        for (int i = 0; i < currentCode.Length; i++)
        {
            if (currentCode[i] == correctCode[i])
            {
                passes++;
                if (passes == currentCode.Length)
                {
                    Debug.Log("Correct combination entered!");
                    PuzzleComplete();
                    return puzzleSolved;
                }
            }
            else
            {
                puzzleSolved = false;
                return false;
            }
        }
        return false;
    }

    private void PuzzleComplete()
    {
        SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
        interactAnimation.enabled = true;
        Destroy(GetComponentInChildren<bikeLockNumber>());
        Destroy(GetComponentInChildren<InteractSound>());
        puzzleSolved = true;
        Disable_Int = true;
        StopInteracting();
    }
}
