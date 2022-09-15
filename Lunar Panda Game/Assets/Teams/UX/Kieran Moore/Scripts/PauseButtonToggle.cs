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
    public bool IsOnRegularMenu;
    public GameObject PauseMenu;
    public FirstPersonController PlayerCharacter;
    //public GameObject BarOfStamina;
    JournalMenuToggle Journal;
    InventoryMenuToggle Inventory;
    FeedbackToggle Feedback;
    public GameObject PauseMenuElement;
    public GameObject InventorySlot;
    public GameObject FlashlightSlot;
    [SerializeField] GameObject firstSelectedButton;

    internal bool canOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter = FindObjectOfType<FirstPersonController>();
        Journal = FindObjectOfType<JournalMenuToggle>();
        Inventory = FindObjectOfType<InventoryMenuToggle>();
        Feedback = FindObjectOfType<FeedbackToggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen)
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (IsPaused == false && Journal.IsOnMenu == false && Inventory.IsOnInventory == false && Feedback.IsOnFeedbackMenu == false)
                {
                    IsPaused = true;
                    //BarOfStamina.SetActive(false);
                    IsOnRegularMenu = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PlayerCharacter.canLook = false;
                    PauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    EventSystem.current.SetSelectedGameObject(null);
                    if (firstSelectedButton != null)
                        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
                    //FindObjectOfType<WalkingSound>().canMakeSound = false;
                    InventorySlot.SetActive(false);
                    FlashlightSlot.SetActive(false);
                  //  SoundEffectManager.SetActive(false);
                }
                else if (IsPaused == true)
                {
                    if (IsOnRegularMenu == true)
                    {
                        Unpause();
                        Cursor.lockState = CursorLockMode.Locked;

                    }
                }
            }


            if (PauseMenuElement.activeInHierarchy == false)
            {
                IsOnRegularMenu = false;
            }


            if (PauseMenuElement.activeInHierarchy == true)
            {
                IsOnRegularMenu = true;
            }
        }
    }

    public void Unpause()
    {
       if (IsOnRegularMenu == true)
        {
            PauseMenu.SetActive(false);
            IsPaused = false;
            //BarOfStamina.SetActive(true);
            IsOnRegularMenu = false;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerCharacter.canLook = true;
            Cursor.visible = false;
            Time.timeScale = 1f;
           // SoundEffectManager.SetActive = true;
            //FindObjectOfType<WalkingSound>().canMakeSound = true;
           InventorySlot.SetActive(true);
             if (tooltipController.FlashlightEnabled == true)
             {
                FlashlightSlot.SetActive(true);
             }
         
            
        }
    }

    public void RestartLvl()
    {
        Unpause();
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
