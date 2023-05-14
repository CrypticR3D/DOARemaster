using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public int cutsceneID;
    //[SerializeField] private int waitTimer = 5;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            if (InteractRaycasting.Instance.raycastInteract(out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    DialogueSystem.Instance.updateDialogue(cutsceneID);
                    DialogueSystem.Instance.playVoiceOver();
                    DisableScript();
                }
            }

            

            //enabled = false;
            //StartCoroutine(DisableScript());


        }

    }

    private void DisableScript()
    {
        this.enabled = false;
    }
    //public IEnumerator DisableScript()
    //{
    //    yield return new WaitForSeconds(waitTimer);
    //    this.enabled = false;
    //}


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueSystem.Instance.updateDialogue(cutsceneID);
            DialogueSystem.Instance.playVoiceOver();
        }
    }
}
