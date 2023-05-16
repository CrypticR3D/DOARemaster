using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesPuzzle : MonoBehaviour
{
    public GameObject Switch_1;
    public GameObject Switch_2;
    public GameObject Switch_3;

    public GameObject Light_1;
    public GameObject Light_2;
    public GameObject Light_3;

    public GameObject Door_1;

    InteractRaycasting raycast;

    private bool Switch_1_isOn;
    private bool Switch_2_isOn;
    private bool Switch_3_isOn;

    private bool Disable_Int;

    [SerializeField] public Animator doorAnim;
    [SerializeField] private string openAnimationName = "DoorOpen";

    private void Awake()
    {
        doorAnim = Door_1.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        raycast = FindObjectOfType<InteractRaycasting>();
        Disable_Int = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (Disable_Int)
            {
                //Do nothing
            }
            else
            {
                ActivateObject();
            }
        }

        if (Switch_1_isOn && Switch_3_isOn || Switch_2_isOn && Switch_3_isOn || Switch_2_isOn && Switch_1_isOn)
        {
            if (Switch_1_isOn && Switch_2_isOn && Switch_3_isOn)
            {
                DisableInteraction();
                DisableAnimation();
                ActivateDoor();
            }
            else
            {
                AllSwitchesOff();
            }
        }
    }
    void ResetSwitch()
    {

    }
    void ActivateDoor()
    {
        doorAnim.Play(openAnimationName);
    }

    void DisableInteraction()
    {
        Disable_Int = true;
    }
    void DisableAnimation()
    {
        Switch_1.GetComponent<InteractAnimation>().enabled = false;
        Switch_2.GetComponent<InteractAnimation>().enabled = false;
        Switch_3.GetComponent<InteractAnimation>().enabled = false;
    }
    void ActivateObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (raycast.raycastInteract(out hit))
        {
            if (hit.transform.gameObject == Switch_1)
            {
                if (Switch_1_isOn)
                {
                    Switch_1_Off();
                }

                else
                {
                    Switch_1_On();
                }
            }
            if (hit.transform.gameObject == Switch_2)
            {
                if (Switch_2_isOn)
                {
                    Switch_2_Off();
                }

                else
                {
                    Switch_2_On();
                }
            }
            if (hit.transform.gameObject == Switch_3)
            {
                if (Switch_3_isOn)
                {
                    Switch_3_Off();
                }

                else
                {
                    Switch_3_On();
                }
            }
        }
    }
    public void Switch_1_On()
    {
        Light_1.GetComponent<Light>().color = Color.green;
        Switch_1_isOn = true;
    }

    public void Switch_2_On()
    {
        Light_2.GetComponent<Light>().color = Color.green;
        Switch_2_isOn = true;
    }

    public void Switch_3_On()
    {
        Light_3.GetComponent<Light>().color = Color.green;
        Switch_3_isOn = true;
    }

    public void Switch_1_Off()
    {
        Light_1.GetComponent<Light>().color = Color.red;
        Switch_1_isOn = false;
    }

    public void Switch_2_Off()
    {
        Light_2.GetComponent<Light>().color = Color.red;
        Switch_2_isOn = false;
    }

    public void Switch_3_Off()
    {
        Light_3.GetComponent<Light>().color = Color.red;
        Switch_3_isOn = false;
    }

    public void AllSwitchesOff()
    {
        Switch_1_Off();
        Switch_2_Off();
        Switch_3_Off();
    }

}
