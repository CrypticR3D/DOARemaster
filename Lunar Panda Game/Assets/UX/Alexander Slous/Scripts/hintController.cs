using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintController : MonoBehaviour
{
    public enum triggerTypes
    {
        NONE, STAY, CONTINUOUS
    }
    [Header("Parameters")]
    [Tooltip("Types:\n" +
        "NONE = Doesn't trigger\n" +
        "STAY = Timer counts down if the player stays in the collider\n" +
        "CONTINUOUS = Timer starts after player enters the collider and doesn't stop")]
    public triggerTypes TriggerType;
    [Tooltip("The time it takes to trigger the hint")]
    public float TimeLimit;
    [Tooltip("What you want the hint to say")]
    public string HintMessage;

    bool TimerActive;
    float TimeLeft;
    public GameObject HintUI;

    private void Start()
    {
        TimeLeft = TimeLimit;
        //HintUI = GameObject.Find("HintText");
    }

    private void Update()
    {
        if (TimerActive == true)
        {
            Debug.Log("Hint timer started");
            TimeLeft -= Time.deltaTime;
        }  

        if (TimeLeft <= 0)
        {
            TimerActive = false;
            ActivateHint();
        }
    }

    void ActivateHint()
    {
        HintUI.SetActive(true);
    }

    //Checks hint type to know how to handle the timer

    void TypeCheck()
    {
        switch (TriggerType)
        {
            case triggerTypes.STAY:
                {
                    TimerActive = false;
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            TimerActive = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            TypeCheck();
        }
    }
}
