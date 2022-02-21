using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSave : MonoBehaviour
{
    internal GameObject player;
    public Inventory inventory;
    public PuzzleData completion;
    internal bool canSave;
    internal Vector3 itemsLocation;

    private void Awake()
    {
        player = FindObjectOfType<playerMovement>().gameObject;
    }

    public void save()
    {
        Database.current.locationUpdate();
        SaveSystem.save(this);
        print("Save");
    }

    public void load()
    {
        GameData data = SaveSystem.load();
        if (data != null)
        {
            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            player.transform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);

            //inventory.itemInventory = data.itemInven;
            int index = 0;

            for(int i = 0; i < inventory.itemInventory.Count; i++)
            {
                inventory.itemInventory[i] = null;
            }

            foreach(string itemId in data.itemInven )
            {
                if(itemId != null)
                {
                    inventory.itemInventory[index] = (Database.current.allItems[int.Parse(itemId)]);
                }

                index += 1;
            }

            index = 0;

            foreach(HoldableItem item in Database.current.itemsInScene)
            {
                if(!inventory.itemInventory.Contains(item.data))
                {
                    if(data.itemsInScene[index,0] != null)
                    {
                        item.gameObject.SetActive(true);
                        item.transform.position = new Vector3((float)data.itemsInScene[index, 0], (float)data.itemsInScene[index, 1], (float)data.itemsInScene[index, 2]);
                        item.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    }
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
                
                index++;
            }

            //Document aka clues or red hering or stuff with images

            index = 0;

            inventory.documentInventory.Clear();

            foreach (string docId in data.docInven)
            {
                if (docId != null)
                {
                    inventory.documentInventory.Add(Database.current.allDocs[int.Parse(docId)]);

                    GameObject document = GameObject.Find(inventory.documentInventory[index].prefab.name);

                    if (document != null)
                    {
                        document.SetActive(false);
                    }
                }

                index += 1;
            }

            index = 0;
            inventory.storyNotesInventory.Clear();

            //Story documents inventory 
            foreach (string storyID in data.docInven)
            {
                if (storyID != null)
                {
                    inventory.storyNotesInventory.Add(Database.current.allStoryNotes[int.Parse(storyID)]);
                    GameEvents.current.onTriggerStoryNotes(Database.current.allStoryNotes[int.Parse(storyID)].id);
                }

                index += 1;
            }

            //Puzzle status
            completion.eventsID = data.puzzlesEvents;
            completion.isCompleted = data.puzzleCompleted;

            print(data.puzzleCompleted.Count);
            for (int i = 0; i < data.puzzleCompleted.Count; i++)
            {
                if (completion.isCompleted[i])
                {
                    GameEvents.current.onPuzzleComplete(i + 1);
                }
                else
                {
                    GameEvents.current.onPuzzleReset(i + 1);
                }
            }
        }
        else
        {
            print("No load data");
        }

    }
}
