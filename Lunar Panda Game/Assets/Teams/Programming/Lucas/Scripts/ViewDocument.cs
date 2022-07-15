using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ViewDocument : MonoBehaviour
{
    internal bool showText = false;
    internal bool showDoc = false;
    internal bool inInventory = false;
    [Tooltip("Enter the corrisponding data (scripable object) for this document")]
    public DocumentData data;

    [Header("Inputs")]
    //public KeyCode keyInteract; //temp
    public KeyCode keyText; //temp
    private bool overDoc; //temp
    private Inventory inventory;
    //internal playerMovement player;
    //internal lockMouse lockMouse;
    internal PlayerCrouch crouchTrigger;
    public FirstPersonController PlayerCharacter;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        //player = FindObjectOfType<playerMovement>();
        //lockMouse = FindObjectOfType<lockMouse>();
        crouchTrigger = FindObjectOfType<PlayerCrouch>();
        PlayerCharacter = FindObjectOfType<FirstPersonController>();

    }

    //temp testing
    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !showDoc)
        {
            RaycastHit hit;
            if(InteractRaycasting.Instance.raycastInteract(out hit))
            {
                if(hit.transform.gameObject == gameObject)
                {
                    UIManager.Instance.toggleMenuVariables();
                    UIManager.Instance.showDocument(data, this);
                    UIManager.Instance.activeImage(data);
                    inInventory = true;
                    //player.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    //player.enabled = false;
                    //lockMouse.canLook = false;
                    PlayerCharacter.canLook = false;
                    //crouchTrigger.enabled = false;
                }
            }
        }
        else if (Input.GetButtonDown("Interact") && showDoc)
        {
            UIManager.Instance.hideDocument(data, this);
            UIManager.Instance.hideText(this);

            //player.enabled = true;
            //lockMouse.canLook = true;
            PlayerCharacter.canLook = true;

            //crouchTrigger.enabled = true;
            UIManager.Instance.toggleMenuVariables();
        }

        if (Input.GetButtonDown("ShowText") && showDoc)
        {
            if(showText)
            {
                UIManager.Instance.hideText(this);
            }
            else
            {
                UIManager.Instance.showingText(data, this);
            }
        }
    }
}
