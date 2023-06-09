using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class PauseButtonToggle : MonoBehaviour
{
    public bool IsPaused;
    public GameObject PauseMenu;
    public FirstPersonController PlayerCharacter;

    UIManager uIManager;

    InventoryMenuToggle Inventory;
    public GameObject PauseMenuElement;

    internal bool canOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter = FindObjectOfType<FirstPersonController>();
        Inventory = FindObjectOfType<InventoryMenuToggle>();
        uIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Pause"))
        {
            if (!IsPaused && !Inventory.IsOnInventory)
            {
                Pause();
            }
            else if (IsPaused)
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        uIManager.HideUI();
        uIManager.isPaused = true;
        uIManager.HideTooltip();

        // Enable cursor and lock it to the screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerCharacter.canLook = false;

        AudioListener.pause = true;
        IsPaused = true;

        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        if (uIManager.isOnPuzzle)
        {
            // Enable cursor and lock it to the screen
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PlayerCharacter.canLook = false;

            uIManager.HideUI();
            uIManager.ShowTooltip();
            //uIManager.isPaused = false;
        }
        else
        {
            // Disable cursor and unlock it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerCharacter.canLook = true;

            uIManager.ShowUI();
            uIManager.HideTooltip();

        }

        AudioListener.pause = false;
        PauseMenu.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;

        uIManager.isPaused = false;
    }

    public void RestartLvl()
    {
        Unpause();
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
