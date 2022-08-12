using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem
{
    public class MyDoorController : MonoBehaviour
    {
        private Animator doorAnim;

        public bool doorOpen = false;
        public bool doorLocked = false;

        [SerializeField] string OpenSound = "DoorOpen";
        [SerializeField] string LockedSound = "DoorLocked";
        [SerializeField] string SlamSound = "DoorSlam";

        [Header("Animation Names")]
        [SerializeField] private string openAnimationName = "DoorOpen";
        [SerializeField] private string closeAnimationName = "DoorClose";
        [SerializeField] private string lockedAnimationName = "doorLocked";
        [SerializeField] private string slamAnimationName = "DoorSlam";

        //[SerializeField] private KeyInventory _keyInventory = null;

        [SerializeField] public Inventory Inv = null;

        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;


        //[SerializeField] ItemData RedKeyData;
        //[SerializeField] ItemData GreenKeyData;
        //[SerializeField] ItemData BlueKeyData;

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

        public void PlayAnimation()
        {

            if (Inv.hasRedKey)
            {
                doorLocked = false;
                OpenDoor();
            }

            if (Inv.hasGreenKey)
            {
                doorLocked = false;
                OpenDoor();
            }

            if (Inv.hasBlueKey)
            {
                doorLocked = false;
                OpenDoor();
            }

            else if (!doorLocked)
            {
                OpenDoor();
            }

            else if (doorLocked && !doorOpen && !pauseInteraction && !Inv.hasRedKey || !Inv.hasGreenKey || !Inv.hasBlueKey)
            {
                LockedDoor();
            }
        }

        public void OpenDoor()
        {
            if (!doorOpen && !doorLocked && !pauseInteraction)
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(OpenSound);
                doorAnim.Play(openAnimationName, 0, 0.0f);
                doorOpen = true;
                StartCoroutine(PauseDoorInteraction());
            }

            else if (doorOpen && !pauseInteraction)
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(OpenSound);
                doorAnim.Play(closeAnimationName, 0, 0.0f);
                doorOpen = false;
                StartCoroutine(PauseDoorInteraction());
            }
        }
        public void LockedDoor()
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(LockedSound);
            doorAnim.Play(lockedAnimationName, 0, 0.0f);
            StartCoroutine(PauseDoorInteraction());
        }

        public void SlamDoor()
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(OpenSound);
            doorAnim.Play(slamAnimationName, 0, 0.0f);
            doorOpen = false;
            StartCoroutine(PauseDoorInteraction());
            StartCoroutine(SlamDoorAudio());
        }
        private IEnumerator SlamDoorAudio()
        {
            yield return new WaitForSeconds(0.7f);
            SoundEffectManager.GlobalSFXManager.PlaySFX(SlamSound);
        }

    }
}