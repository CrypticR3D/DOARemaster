using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeySystem
{
    public class    KeyItemController : MonoBehaviour
    {
        [SerializeField] private bool redDoor = false;
        [SerializeField] private bool greenDoor = false;
        [SerializeField] private bool blueDoor = false;

        private MyDoorController doorObject;

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

            //Green Door//
            if (greenDoor)
            {
                doorObject.PlayAnimation();
            }

            //Blue Door//
            if (blueDoor)
            {
                doorObject.PlayAnimation();
            }
        }
    }
}