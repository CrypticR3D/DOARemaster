using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesPuzzle : MonoBehaviour
{
    public GameObject Switch_1;
    public GameObject Switch_2;
    public GameObject Switch_3;

    public GameObject Object_1;
    public GameObject Object_2;
    public GameObject Object_3;

    InteractRaycasting raycast;

    private bool Switch_1_isOn;
    private bool Switch_2_isOn;
    private bool Switch_3_isOn;

    // Start is called before the first frame update
    void Start()
    {
        raycast = FindObjectOfType<InteractRaycasting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            ActivateObject();
        }
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
        Object_1.GetComponent<Light>().color = Color.green;
        Switch_1_isOn = true;
    }

    public void Switch_2_On()
    {
        Object_2.GetComponent<Light>().color = Color.green;
        Switch_2_isOn = true;
    }

    public void Switch_3_On()
    {
        Object_3.GetComponent<Light>().color = Color.green;
        Switch_3_isOn = true;
    }

    public void Switch_1_Off()
    {
        Object_1.GetComponent<Light>().color = Color.red;
        Switch_1_isOn = false;
    }

    public void Switch_2_Off()
    {
        Object_2.GetComponent<Light>().color = Color.red;
        Switch_2_isOn = false;
    }

    public void Switch_3_Off()
    {
        Object_3.GetComponent<Light>().color = Color.red;
        Switch_3_isOn = false;
    }

}
