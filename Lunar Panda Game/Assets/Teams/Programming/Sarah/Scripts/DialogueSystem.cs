using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public struct cutscene
    {
        public List<Dialogue> script;
    }

    public static DialogueSystem Instance;

    public List<cutscene> Cutscenes;
    internal Dialogue currentDialogue;
    int currentCutsceneID = 0;
    public AudioSource voiceOverSource;

    [SerializeField] private int waitTimer = 1;


    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void updateDialogue(int cutsceneID)
    {
        currentCutsceneID = cutsceneID;
    }

    public void playVoiceOver()
    {
        StartCoroutine(voiceOver());
    }

    IEnumerator voiceOver()
    {
        foreach (Dialogue dialogue in Cutscenes[currentCutsceneID].script)
        {
            print(dialogue.script);
            //voiceOverSource.clip = dialogue.voiceOver;
            //voiceOverSource.Play();
            UIManager.Instance.textToScreen(dialogue.script);

            yield return new WaitForSeconds(waitTimer);
            yield return null;

            //if (!voiceOverSource.isPlaying)
            //{
            //    print(dialogue.script);
            //    voiceOverSource.clip = dialogue.voiceOver;
            //    voiceOverSource.Play();
            //    UIManager.Instance.textToScreen(dialogue.script);
                
            //    yield return new WaitForSeconds(waitTimer);

            //    while (voiceOverSource.isPlaying)
            //    {
            //        yield return null;
            //    }
            //}
        }

        UIManager.Instance.diableSubtitles();
    }
}
