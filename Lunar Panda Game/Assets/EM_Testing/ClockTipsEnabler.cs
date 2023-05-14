using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTipsEnabler : MonoBehaviour
{
    // Start is called before the first frame update


    Inventory inventory;
    //[SerializeField] ItemData clockHandsData;

    ClockPuzzle clockPuzzle;

    [SerializeField] public GameObject FirstTrigger;

    [SerializeField] public GameObject SecondTrigger;

    [SerializeField] public bool doOnce;

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        clockPuzzle = FindObjectOfType<ClockPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clockPuzzle.handsConnected)
        {
            if (doOnce == false)
            {
                EnableTips();
            }
            doOnce = true;
        }
    }

    void EnableTips()
    {
        FirstTrigger.SetActive(false);
        SecondTrigger.SetActive(true);

    }
}
