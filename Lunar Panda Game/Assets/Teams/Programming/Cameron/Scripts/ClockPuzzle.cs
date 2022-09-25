using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockPuzzle : MonoBehaviour
{
    [SerializeField] Animation anim;
    public string rotateAudio;//Matej edit
    public string placeAudio;//Matej edit
    public string chimeAudio;//Matej edit
    public bool handsConnected = false;
    const float minuteAngle = 30f;
    const float hourAngle = 270f;

    [SerializeField] GameObject hourHand;

    [SerializeField] GameObject minuteHand;

    [SerializeField] GameObject clockHands;

    [SerializeField] ItemData clockHandsData;

    Inventory inventory;

    [SerializeField] float desiredMinuteRotation;

    [SerializeField] float desiredHourRotation;
    
    [SerializeField] Transform door;
    [SerializeField] Transform pivot;

    [SerializeField] public Transform Handpivot;
    private bool completed;

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    //[SerializeField] ToolTipType tooltip;
    //[SerializeField] Text tooltipTxt;
    //[SerializeField] CanvasGroup tooltipGroup;
    //PlayerPickup pickup;
    //[SerializeField] float waitTime = 0.5f;

    bool isFading = false;


    /*[Tooltip("This is the position of the minute hand when its placed on the clock")]
    [SerializeField] Vector3 minHandClockPos;
    [Tooltip("This is the position of the hour hand when its placed on the clock")]
    [SerializeField] Vector3 hourHandClockPos;*/

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (!completed)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        PlaceHands();
                        SoundEffectManager.GlobalSFXManager.PlaySFX(placeAudio);//Matej edit
                    }
                }

                else
                {
                    return;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                if (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        if (handsConnected)
                        {
                            RotateHand();
                            StartCoroutine(PauseInteraction());
                            //StartCoroutine(FadeTooltips());
                        }
                    }
                }

                else
                {
                    return;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                if (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        if (handsConnected)
                        {
                            RotateHandOtherway();
                            StartCoroutine(PauseInteraction());
                            //StartCoroutine(FadeTooltips());
                        }
                    }
                }

                else
                {
                    return;
                }
            }
        }
    }



    void RotateHand()
    {
        //minuteHand.transform.eulerAngles = new Vector3(minuteHand.transform.eulerAngles.x, minuteHand.transform.eulerAngles.y, minuteHand.transform.eulerAngles.z + clockAngle);
        minuteHand.transform.RotateAround(Handpivot.position, -Vector3.right , minuteAngle);
        //the issue with just doing if minute hand is at 90 degrees is that float precision is a piece
        
        if(minuteHand.transform.eulerAngles.z > hourAngle - 1 && minuteHand.transform.eulerAngles.z < hourAngle + 1)
        {
            hourHand.transform.RotateAround(Handpivot.position, -Vector3.right, minuteAngle);
        }

        CheckCombination();
    }
    void RotateHandOtherway()
    {
        //minuteHand.transform.eulerAngles = new Vector3(minuteHand.transform.eulerAngles.x, minuteHand.transform.eulerAngles.y, minuteHand.transform.eulerAngles.z + clockAngle);
        minuteHand.transform.RotateAround(Handpivot.position, Vector3.right, minuteAngle);
        //the issue with just doing if minute hand is at 90 degrees is that float precision is a piece

        if (minuteHand.transform.eulerAngles.z > hourAngle + 1 && minuteHand.transform.eulerAngles.z < hourAngle - 1)
        {
            hourHand.transform.RotateAround(Handpivot.position, Vector3.right, minuteAngle);
        }

        CheckCombination();
    }
    //IEnumerator FadeTooltips()
    //{
    //    tooltipTxt.text = tooltip.text;
    //    for (float t = 0f; t < tooltip.fadeTime; t += Time.deltaTime)
    //    {
    //        float normalizedTime = t / tooltip.fadeTime;
    //        tooltipGroup.alpha = Mathf.Lerp(0, 1, normalizedTime);
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(waitTime);
    //    while (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))
    //    {
    //        if (hit.transform.gameObject == gameObject && pickup.heldItem == null)
    //        {
    //            yield return new WaitForEndOfFrame();
    //            yield return null;
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //    isFading = false;
    //    for (float t = 0f; t < tooltip.fadeTime; t += Time.deltaTime)
    //    {
    //        float normalizedTime = t / tooltip.fadeTime;
    //        tooltipGroup.alpha = Mathf.Lerp(1, 0, normalizedTime);
    //        yield return null;
    //    }
    //    tooltipGroup.alpha = 0;
    //    yield return null;
    //}

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        if (pauseInteraction)
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(rotateAudio);//Matej edit
            yield return new WaitForSeconds(waitTimer);
        }

        else
        {
            pauseInteraction = false;
        }
    }

    private IEnumerator UnlockDoor()
    {
        //unlock the door
        SoundEffectManager.GlobalSFXManager.PlaySFX(chimeAudio);//Matej edit
        yield return new WaitForSeconds(7);
        anim.Play();
    }

    void PlaceHands()
    {
        if(inventory.itemInventory[inventory.selectedItem] == clockHandsData)
        {
            clockHands = Instantiate(clockHands, Handpivot.position, Quaternion.Euler(0, -90, 0));

            Destroy(clockHands.GetComponent<HoldableItem>());
            Destroy(clockHands.GetComponent<Rigidbody>());
            Destroy(clockHands.GetComponent<BoxCollider>());
            //Destroy(clockHands.GetComponent<Rigidbody>());
            minuteHand = clockHands.transform.GetChild(0).gameObject;
            //Destroy(minuteHand.GetComponent<Rigidbody>());
            //Destroy(minuteHand.GetComponent<GlowWhenLookedAt>());
            Destroy(minuteHand.GetComponent<HoldableItem>());
            //Destroy(minuteHand.GetComponent<Collider>());
            hourHand = clockHands.transform.GetChild(1).gameObject;
            //Destroy(hourHand.GetComponent<Rigidbody>());
            //Destroy(hourHand.GetComponent<GlowWhenLookedAt>());
            Destroy(hourHand.GetComponent<HoldableItem>());
            //Destroy(hourHand.GetComponent<Collider>());
            //remove the current selected item (clock hands) from the inventory
            inventory.removeItem();
            handsConnected = true;
            clockHands.SetActive(true);

            if (FindObjectOfType<PlayerPickup>().heldItem != null)
            {
                if (FindObjectOfType<PlayerPickup>().heldItem.name == "Hand_Min" || FindObjectOfType<PlayerPickup>().heldItem.name == "Hand_Hour")
                {
                    //idk why this is an issue, but it is. Maybe a bug with the player pickup script? or inventory script? or this script? No clue tbh
                    FindObjectOfType<PlayerPickup>().heldItem = null;
                }
            }
        }
    }
    void CheckCombination()
    {
        //if both the minute and hour hand are at the desired rotation
        if (minuteHand.transform.eulerAngles.z > desiredMinuteRotation - 1 && minuteHand.transform.eulerAngles.z < desiredMinuteRotation + 1)
        {
            if (hourHand.transform.eulerAngles.z > desiredHourRotation - 1 && hourHand.transform.eulerAngles.z < desiredHourRotation + 1)
            {
                completed = true;
                StartCoroutine(UnlockDoor());

                if (Analysis.current != null)
                {
                    if (Analysis.current.consent && (!Analysis.current.timersPuzzlesp1.ContainsKey("Clock") && !Analysis.current.timersPuzzlesp1.ContainsKey("Clock")))
                    {
                        Analysis.current.resetTimer("Clock");
                    }
                }
            }
        }
    }
}