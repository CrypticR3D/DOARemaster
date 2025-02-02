using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    bool moving = false;
    public int moveTotal;
    public bool isPositiveRotation; 
    int currentTotal;
    public bool canOpen;
    bool beenOpened = false;
    public string openClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            currentTotal += 1;

            if(isPositiveRotation)
            {
                transform.Rotate(new Vector3(0, 1, 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, -1, 0));
            }

            if (moveTotal == currentTotal)
            {
                moving = false;
                currentTotal = 0;
                beenOpened = true;
            }
        }

        if(canOpen && !moving && !beenOpened)
        {
            if (Input.GetButton("Interact"))
            {
                RaycastHit hit;
                if (InteractRaycasting.Instance.raycastInteract(out hit))
                {
                    if (hit.transform.gameObject == gameObject.transform.GetChild(0).gameObject)
                    {
                        SoundEffectManager.GlobalSFXManager.PlaySFX(openClip);
                        moving = true;
                    }
                }
            }
        }
    }
}
