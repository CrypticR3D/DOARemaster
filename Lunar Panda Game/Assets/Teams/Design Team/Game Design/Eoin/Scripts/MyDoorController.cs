using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    private Animator doorAnim;

    private bool doorOpen = false;

    [SerializeField] string InteractSound = "Electric_Switch";

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    //bool AnimatorIsPlaying()
    //{

    //    if (doorAnim != null)
    //    {
    //        print(doorAnim.GetCurrentAnimatorStateInfo(0).length > doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    //        return doorAnim.GetCurrentAnimatorStateInfo(0).length > doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    //    }
    //    else
    //        return false;
    //}

    public void PlayAnimation()
    {
        SoundEffectManager.GlobalSFXManager.PlaySFX(InteractSound);

        if (!doorOpen)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("DoorClose", 0, 0.0f);
            doorOpen = false;        
        }
    }


}
