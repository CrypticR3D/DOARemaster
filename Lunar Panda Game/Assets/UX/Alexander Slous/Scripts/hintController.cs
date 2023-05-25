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
        "CONTINUOUS = Timer counts down after player enters the collider and doesn't stop")]
    public triggerTypes TriggerType;
    [Tooltip("The time it takes to trigger the hint")]
    public float TimeLimit;
    [Tooltip("What you want the hint to say")]
    public string HintText;

    bool TimerActive;
    float TimeLeft;

    private void Start()
    {
        TimeLeft = TimeLimit;
    }

    private void Update()
    {
        if (TimerActive == true)
        {
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

    }

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
