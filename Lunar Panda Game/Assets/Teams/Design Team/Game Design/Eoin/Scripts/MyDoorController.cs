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

        [Header("Animation Names")]
        [SerializeField] private string openAnimationName = "DoorOpen";
        [SerializeField] private string closeAnimationName = "DoorClose";
        [SerializeField] private string lockedAnimationName = "doorLocked";

        [SerializeField] private KeyInventory _keyInventory = null;

        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;

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

            if (_keyInventory.hasRedKey)
            {
                OpenDoor();
            }

            else if (_keyInventory.hasBlueKey)
            {
                OpenDoor();
            }

            else if (_keyInventory.hasGreenKey)
            {
                OpenDoor();
            }

            else if (doorLocked && !doorOpen && !pauseInteraction || !_keyInventory.hasRedKey || !_keyInventory.hasGreenKey || !_keyInventory.hasBlueKey)
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(LockedSound);
                doorAnim.Play(lockedAnimationName, 0, 0.0f);
                StartCoroutine(PauseDoorInteraction());
            }

        }

        private void OpenDoor()
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
    }

}