using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PadlockInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public Camera lockCamera;
    public KeyCode interactKey = KeyCode.E;
    public string correctCombination = "1234";

    private string enteredCombination = "";
    private bool interacting = false;

    [Header("Codes")]
    [Tooltip("The combination that solves the puzzle")]
    public int[] correctCode;
    [Tooltip("The combination displayed before the player has interacted with it")]
    public int[] currentCode;

    [Header("Puzzle State")]
    [Tooltip("Check for if the puzzle is solved")]
    public bool puzzleSolved;

    private void Start()
    {
        // Disable the lock camera at the start
        lockCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (interacting)
        {
            // Check for combination input
            checkPuzzleComplete();

            // Check for backing away from the lock
            if (Input.GetKeyDown(interactKey))
            {
                StopInteracting();
            }
        }
        else
        {
            // Check for starting interaction
            if (Input.GetKeyDown(interactKey))
            {
                StartInteracting();
            }
        }
    }

    private void StartInteracting()
    {
        // Enable the lock camera and disable the main camera
        mainCamera.gameObject.SetActive(false);
        lockCamera.gameObject.SetActive(true);

        // Enable cursor and lock it to the screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        interacting = true;
    }

    private void StopInteracting()
    {
        // Reset the entered combination
        enteredCombination = "";

        // Disable the lock camera and enable the main camera
        lockCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Disable cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        interacting = false;
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
        // Check if the entered combination is correct
        if (enteredCombination == correctCombination)
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
                        puzzleSolved = true;
                        StopInteracting();
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
        return false;
    }
}
