using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bikeLock : MonoBehaviour
{
    //[SerializeField] InteractAnimation interactAnimation;

    //public string audioClipName;
    [Header("Codes")]
    [Tooltip("The combination that solves the puzzle")]
    public int[] correctCode;
    [Tooltip("The combination displayed before the player has interacted with it")]
    public int[] currentCode;

    [Header("Puzzle State")]
    [Tooltip("Check for if the puzzle is solved")]
    public bool puzzleSolved;

    bool finished;

    public Camera mainCamera; // Reference to the main camera
    public GameObject puzzleCamera; // Reference to the camera for the puzzle
    bool puzzleInteracted = false;

    public GameObject lockNumber_1;
    public GameObject lockNumber_2;
    public GameObject lockNumber_3;
    public GameObject lockNumber_4;

    private Vector3 cameraStartPosition; // The starting position of the player's camera

    void Start()
    {
        Cursor.visible = false; // Hide the cursor at the start
        cameraStartPosition = mainCamera.transform.position; // Store the starting position of the player's camera
    }

    void Update()
    {
        SetCollision();

        if (puzzleSolved)
        {
            DisablePuzzle();
        }

        if (Input.GetMouseButtonDown(0) && !puzzleInteracted)
        {
            Cursor.lockState = CursorLockMode.None;
            puzzleInteracted = true;
        }
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
                    puzzleSolved = true;
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

    void SetCollision()
    {
        if (puzzleSolved && !finished)
        {
            //SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
            //interactAnimation.enabled = true;
            finished = true;
        }
    }

    private void DisablePuzzle()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(GetComponentInChildren<bikeLockNumber>());
        lockNumber_1.GetComponent<Collider>().enabled = false;
        lockNumber_2.GetComponent<Collider>().enabled = false;
        lockNumber_3.GetComponent<Collider>().enabled = false;
        lockNumber_4.GetComponent<Collider>().enabled = false;
        Destroy(GetComponentInChildren<InteractSound>());
        Cursor.lockState = CursorLockMode.Locked;
        puzzleInteracted = false;
        enabled = false;
    }
}
