using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryMenuToggle : MonoBehaviour
{

    public bool IsOnInventory;

    [SerializeField] GameObject InventoryMenu;
    PauseButtonToggle Pause;
    PlayerPickup pickup;
    UIManager uIManager;

    public FirstPersonController PlayerCharacter;

    internal bool canOpen = true;


    // Start is called before the first frame update
    void Start()
    {
        Pause = FindObjectOfType<PauseButtonToggle>();
        pickup = FindObjectOfType<PlayerPickup>();
        PlayerCharacter = FindObjectOfType<FirstPersonController>();
        uIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen)
        {
            if (Input.GetButtonDown("Inventory"))
            {
                //Shows the inventory
                if (IsOnInventory == false && !Pause.IsPaused)
                {
                    uIManager.isOnInventory = true;
                    uIManager.HideUI();
                    IsOnInventory = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PlayerCharacter.canLook = false;
                    pickup.enabled = false;
                    InventoryMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
                //Hides the inventory
                else if (IsOnInventory == true)
                {
                    uIManager.isOnInventory = false;
                    uIManager.ShowUI();
                    InventoryMenu.SetActive(false);
                    IsOnInventory = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    PlayerCharacter.canLook = true;
                    Cursor.visible = false;
                    pickup.enabled = true;
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
