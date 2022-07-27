using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeySystem
{
    public class KeyItemController : MonoBehaviour
    {
        [SerializeField] private bool redDoor = false;
        [SerializeField] private bool greenDoor = false;
        [SerializeField] private bool blueDoor = false;

        [SerializeField] private bool redKey = false;
        [SerializeField] private bool greenKey = false;
        [SerializeField] private bool blueKey = false;

        [SerializeField] private KeyInventory _keyInventory = null;

        private MyDoorController doorObject;
        public Inventory Inv;

        [SerializeField] ItemData RedKeyData;
        [SerializeField] ItemData GreenKeyData;
        [SerializeField] ItemData BlueKeyData;

        void Awake()
        {
            Inv = FindObjectOfType<Inventory>();
        }

        private void Start()
        {
            if (redDoor || greenDoor || blueDoor)
            {
                doorObject = GetComponent<MyDoorController>();
            }
            
        }

        public void ObjectInteraction()
        {

            //Red Door//
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }

            else if (redKey)
            {
                _keyInventory.hasRedKey = true;

                //if (Inv.itemInventory[Inv.selectedItem] == RedKeyData)
                //{
                //    _keyInventory.hasRedKey = true;
                //}
                //else if 
                //{
                //    _keyInventory.hasRedKey = false;
                //}
            }

            //Green Door//
            else if(greenDoor)
            {
                doorObject.PlayAnimation();
            }

            else if (greenKey)
            {
                _keyInventory.hasGreenKey = true;

                //if (Inv.itemInventory[Inv.selectedItem] == GreenKeyData)
                //{
                //    _keyInventory.hasGreenKey = true;
                //}
                //else
                //{
                //    _keyInventory.hasGreenKey = false;
                //}
            }

            //Blue Door//
            else if(blueDoor)
            {
                doorObject.PlayAnimation();
            }

            else if (blueKey)
            {
                _keyInventory.hasBlueKey = true;

                //if (Inv.itemInventory[Inv.selectedItem] == BlueKeyData)
                //{
                //    _keyInventory.hasBlueKey = true;
                //}
                //else
                //{
                //    _keyInventory.hasBlueKey = false;
                //}
            }
        }
    }
}