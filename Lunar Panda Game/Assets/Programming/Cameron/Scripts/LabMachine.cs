/*
For this script i need to make it so that when one combination on the switch wall is completed,
the tube thingies change material or light up or something. Should do it in this script to make
it more object oriented, but idk if i can be bothered or if its practical to do so
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabMachine : MonoBehaviour
{
    //Filip Changes
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public bool has3DAudio;

    internal int id;
    //[SerializeField] List<GameObject> tubes;
    [SerializeField] List<Light> antidoteLights;
    //[SerializeField] List<Material> completeTubeMats;
    [SerializeField] SwitchWall switchWall;
    [SerializeField] GameObject button;
    [SerializeField] ItemData antidoteData;
    [SerializeField] GameObject antidote;
    [SerializeField] Material antidoteGlowMat;
    Countdown countdown;
    bool antidoteMade;
    Transform cam;
    Transform player;
    Vector3 startAntidote;

    void Awake()
    {
        countdown = FindObjectOfType<Countdown>();
        cam = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //GameEvents.current.puzzleReset += ResetMachine;
        id = switchWall.id;
        startAntidote = antidote.transform.position;
    }

    void Update()
    {
        RaycastHit hit;
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, player.GetComponent<PlayerPickup>().pickupDist))
            {
                if (hit.transform.gameObject == button)
                {
                    //if all the combos have been put in and are fine.
                    if (switchWall.CheckAllCombos())
                    {
                        //Filip Changes
                        if (has3DAudio)
                            audioSource2.Play();

                        antidoteMade = true;
                        //change the antidote so that it can be picked up and stuff
                        //add holdableitem and item data

                        HoldableItem holdableItem = antidote.AddComponent<HoldableItem>();
                        holdableItem.data = antidoteData;

                        //add glowyinteractthingie
                        GlowWhenLookedAt glowy = antidote.AddComponent<GlowWhenLookedAt>();
                       // glowy.glowingMaterial = antidoteGlowMat;
                        antidote.AddComponent<Rigidbody>();
                        antidote.transform.parent = null;
                       
                        //Stop countdown
                        countdown.StopTimer();

                        Database.current.addToItemsInScene(holdableItem);
                        Database.itemLocation.Add(holdableItem.gameObject.transform.position);



                    }
                }
            }
        }
    }

    //tube num has to be 0, 1, or 2
    public void TurnTubeOn(bool[] completedCombos)
    {
        //goes through the bool array of completed combos, and if any are true, change em to be "on"
        for(int i = 0; i < completedCombos.Length; i++)
        {
            if (completedCombos[i])
            {
                //Filip Changes
                if (has3DAudio)
                    audioSource.Play();

                antidoteLights[i].GetComponent<Light>().enabled = true;
                //I think they want to play a sound here
            }
        }
    }

    public void ResetMachine(int id)
    {
        if(id == switchWall.id)
        {
            for (int i = 0; i < antidoteLights.Count; i++)
            {
                antidoteLights[i].GetComponent<Light>().enabled = false;
            }

            Database.current.itemsInScene.Remove(antidote.GetComponent<HoldableItem>());
            antidote.transform.position = startAntidote;
        }
    }
}
