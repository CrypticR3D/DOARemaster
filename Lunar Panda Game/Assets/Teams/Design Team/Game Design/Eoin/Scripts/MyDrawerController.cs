using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDrawerController : MonoBehaviour
{
    [SerializeField] public Animator drawerAnim;

    public bool drawerOpen = false;
    public bool drawerLocked = false;

    [SerializeField] string OpenSound = "DoorOpen";
    [SerializeField] string LockedSound = "DoorLocked";

    [SerializeField] private string openAnimationName = "drawerOpen";
    [SerializeField] private string closeAnimationName = "drawerClose";
    [SerializeField] private string lockedAnimationName = "drawerLocked";

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    public GameObject Parent2Drawer;

    private void Awake()
    {
        drawerAnim = gameObject.GetComponent<Animator>();
    }


    private IEnumerator PauseDrawerInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void PlayAnimation()
    {

        if (drawerLocked & !drawerOpen && !pauseInteraction)
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(LockedSound);
            drawerAnim.Play(lockedAnimationName, 0, 0.0f);
            StartCoroutine(PauseDrawerInteraction());
        }

        if (!drawerOpen & !drawerLocked && !pauseInteraction)
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(OpenSound);
            drawerAnim.Play(openAnimationName, 0, 0.0f);
            drawerOpen = true;
            Parent2Drawer.SetActive(false);
            StartCoroutine(PauseDrawerInteraction());
        }

        else if (drawerOpen && !pauseInteraction)
        {
            SoundEffectManager.GlobalSFXManager.PlaySFX(OpenSound);
            drawerAnim.Play(closeAnimationName, 0, 0.0f);
            drawerOpen = false;
            Parent2Drawer.SetActive(true);
            StartCoroutine(PauseDrawerInteraction());
        }
    }
}