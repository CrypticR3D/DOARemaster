using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KeySystem;

public class autoCombineScript : MonoBehaviour
{
    private string combinePrompt = "You combine the items together to create: ";
    private string itemNamePrompt;

    [System.Serializable]
    public class itemBuildPath
    {
        public List<ItemData> itemParts;
        public ItemData combinedItem;
        public GameObject instance;
    }

    [Header("Items Required")]
    [Tooltip("The list of all items that can be made through combining and their ingredients")]
    public List<itemBuildPath> autoCombineItemsList;

    public Inventory inventoryScript;
    private List<List<bool>> inInventory;

    [SerializeField] ItemData RedKeyData;
    [SerializeField] private KeyInventory _keyInventory;

    void Start()
    {
        inInventory = new List<List<bool>>();
        List<bool> temp = new List<bool>();
        inventoryScript = FindObjectOfType<Inventory>();
        for (int j = 0; j < autoCombineItemsList.Count; j++)
        {
            temp = new List<bool>();
            for (int i = 0; i < autoCombineItemsList[j].itemParts.Count; i++)
            {
                temp.Add(false);
            }

            inInventory.Add(temp);
        }
    }

    public void itemChecking(ItemData item)
    {
        for (int j = 0; j < autoCombineItemsList.Count; j++)
        {
            for (int i = 0; i < autoCombineItemsList[j].itemParts.Count; i++)
            {
                if (autoCombineItemsList[j].itemParts[i] == item)
                {
                    inInventory[j][i] = true;
                }
            }
        }
        combine();
    }

    bool canCombine(int j)
    {
        for (int i = 0; i < inInventory[j].Count; i++)
        {
            if (!inInventory[j][i])
            {
                return false;
            }
        }

        return true;
    }

    void combine()
    {
        for (int k = 0; k < autoCombineItemsList.Count; k++)
        {
            if (canCombine(k) == true)
            {
                for (int i = 0; i < inventoryScript.itemInventory.Count; i++)
                {
                    for (int j = 0; j < autoCombineItemsList[k].itemParts.Count; j++)
                    {
                        if (autoCombineItemsList[k].itemParts[j] == inventoryScript.itemInventory[i])
                        {
                            inventoryScript.itemInventory[i] = null;
                            inInventory[k][j] = false;
                        }
                    }
                }

                inventoryScript.addItem(autoCombineItemsList[k].combinedItem);
                itemNamePrompt = autoCombineItemsList[k].combinedItem.itemName;
                Debug.Log(combinePrompt + itemNamePrompt);
            }
        }
    }
}