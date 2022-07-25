using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            if (redDoor)
            {
                doorObject = GetComponent<MyDoorController>();
            }
            
        }

        public void ObjectInteraction()
        {
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }

            else if (redKey)
            {
                _keyInventory.hasRedKey = true;
                gameObject.SetActive(false);
            }
        }

    }
}