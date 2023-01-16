using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EM_Inventory : MonoBehaviour
{

    public static EM_Inventory instance;
    public List<Item> playerInventory = new List<Item>();
    public Action onItemChangedCallback;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddItem(Item itemToAdd)
    {
        playerInventory.Add(itemToAdd);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(Item itemToRemove)
    {
        playerInventory.Remove(itemToRemove);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }


}

public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int itemID;
    public int itemAmount;
    public int itemValue;
    public bool stackable;
    public ItemType itemType;

    public Item(string name, string desc, Sprite icon, int id, int amount, int value, bool stack, ItemType type)
    {
        itemName = name;
        itemDescription = desc;
        itemIcon = icon;
        itemID = id;
        itemAmount = amount;
        itemValue = value;
        stackable = stack;
        itemType = type;
    }
}
public enum ItemType
{
    Weapon,
    Consumable,
    Quest,
    Armor
}