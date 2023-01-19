using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KeySystem;

public class Inventory : MonoBehaviour
{
    //inventory system that can hold multiple items 
    [SerializeField] 
    //internal List<ItemData> itemInventory;
    public List<ItemData> itemInventory;
    [SerializeField] 
    internal List<DocumentData> documentInventory;
    [SerializeField]
    internal List<StoryData> storyNotesInventory;
    internal int selectedItem = 0;
    private autoCombineScript autoCombine;
    public int maxItemsInInventory = 12;

    private int itemsIn;
    PlayerPickup pickupControl;
    public GameObject player;
    internal bool puttingAway = false;
    GameObject puttingAwayItem;
    float lerpSpeed = 5;
    Camera cam;
    float timer = 0;
    public float maxTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.docInventory.Count != 0)
        {
            documentInventory = GameManager.Instance.docInventory;
        }

        player = FindObjectOfType<CharacterController>().gameObject;
        pickupControl = FindObjectOfType<PlayerPickup>();
        itemInventory = new List<ItemData>();
        documentInventory = new List<DocumentData>();
        autoCombine = FindObjectOfType<autoCombineScript>();
        cam = FindObjectOfType<Camera>();

        for(int i = 0; i < maxItemsInInventory; i++)
        {
            itemInventory.Add(null);
        }

    }

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && !pickupControl.holdingNarrative)
        {
            selectItem(true);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && !pickupControl.holdingNarrative)
        {
            selectItem(false);
        }
        
        if(Input.GetButtonDown("Interact") && itemInventory[selectedItem] != null)
        {
            if (pickupControl.heldItem != null)
            {
                if (!pickupControl.holdingNarrative)
                {
                    itemInventory[selectedItem].timesUses++;
                }
            }

        }
    }

    public void addItem(ItemData data)
    {
        bool notInInventory = true; 
        for (int i = 0; i < itemInventory.Count; i++)
        {
            if(itemInventory[i] == data)
            {
                notInInventory = false;
            }
        }

        if (notInInventory)
        {
            for (int i = 0; i < itemInventory.Count; i++)
            {
                if (itemInventory[i] == null)
                {
                    itemInventory[i] = data;
                    UIManager.Instance.inventoryItemAdd(data, i);
                    itemsIn++;
                    break;
                }
            }
            data.beenPickedUp = true;
            UIManager.Instance.inventoryItemSelected(itemInventory[selectedItem], selectedItem);
            UIManager.Instance.itemEquip(itemInventory[selectedItem]);
            autoCombine.itemChecking(data);
        }
    }

    public void addItem(DocumentData data)
    {
        documentInventory.Add(data);
        GameManager.Instance.docInventory.Add(data);
        data.beenPickedUp = true;
    }

    public void addItem(StoryData data)
    {
        storyNotesInventory.Add(data);
        data.beenPickedUp = true;

    }

    public void removeItem()
    {
        //If item is used then it will not be dropped on the floor
        itemInventory[selectedItem] = null;
        itemsIn--;

        UIManager.Instance.inventoryItemSelected(itemInventory[selectedItem], selectedItem);
        UIManager.Instance.itemEquip(itemInventory[selectedItem]);
    }

    private void selectItem(bool positive)
    {
        if(positive)
        {
            selectedItem++;
        }
        else
        {
            selectedItem--;
        }


       if (selectedItem > maxItemsInInventory - 1)
        {
            selectedItem = 0;
        }

       if(selectedItem < 0)
        {
            selectedItem = maxItemsInInventory - 1;
        }

        UIManager.Instance.inventoryItemSelected(itemInventory[selectedItem], selectedItem);
        UIManager.Instance.itemEquip(itemInventory[selectedItem]);
    }

    public void selectItem(int inventoryNumber)
    {
        selectedItem = inventoryNumber;
        UIManager.Instance.inventoryItemSelected(itemInventory[selectedItem], inventoryNumber);
        UIManager.Instance.itemEquip(itemInventory[selectedItem]);
    }
}