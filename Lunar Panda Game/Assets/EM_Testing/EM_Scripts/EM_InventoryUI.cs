using UnityEngine;
using UnityEngine.UI;
using System;


public class EM_InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    EM_Inventory inventory;

    EM_InventorySlot[] slots;

    void Start()
    {
        inventory = EM_Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<EM_InventorySlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < Mathf.Min(slots.Length, inventory.playerInventory.Count); i++)
        {
            if (i < inventory.playerInventory.Count)
            {
                slots[i].AddItem(inventory.playerInventory[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
