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
    //lockMouse mouseLock;
    //public GameObject BarOfStamina;
    JournalMenuToggle Journal;
    PauseButtonToggle Pause;
    FeedbackToggle Feedback;
    PlayerPickup pickup;

    public FirstPersonController PlayerCharacter;

    internal bool canOpen = true;
    [SerializeField] GameObject TorchUI;
    [SerializeField] GameObject InvPanUI;

    //public AudioMixerGroup NSfX;

    //string VolumeMute;
    // Start is called before the first frame update
    void Start()
    {
        //mouseLock = FindObjectOfType<lockMouse>();
        Pause = FindObjectOfType<PauseButtonToggle>();
        Journal = FindObjectOfType<JournalMenuToggle>();
        Feedback = FindObjectOfType<FeedbackToggle>();
        pickup = FindObjectOfType<PlayerPickup>();
        PlayerCharacter = FindObjectOfType<FirstPersonController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen)
        {
            if (Input.GetButtonDown("Inventory"))
            {
                //if the inventory ui isnt on screen
                if (IsOnInventory == false && Journal.IsOnMenu == false && Feedback.IsOnFeedbackMenu == false && !Pause.IsPaused)
                {
                    //NSfX.audioMixer.SetFloat(VolumeMute, 0.0f);
                    TorchUI.SetActive(false);
                    InvPanUI.SetActive(false);
                    IsOnInventory = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    PlayerCharacter.canLook = false;
                    pickup.enabled = false;
                    //mouseLock.canLook = false;
                    InventoryMenu.SetActive(true);
                    Time.timeScale = 0f;


                }
                //if the inventory ui is already on screen
                else if (IsOnInventory == true)
                {
                    //M
                    TorchUI.SetActive(true);
                    InvPanUI.SetActive(true);
                    InventoryMenu.SetActive(false);
                    IsOnInventory = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    PlayerCharacter.canLook = true;
                    //mouseLock.canLook = true;
                    Cursor.visible = false;
                    pickup.enabled = true;
                    Time.timeScale = 1f;

                }
            }
        }
    }
}
