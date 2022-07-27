using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeySystem
{
    public class InteractRaycast : MonoBehaviour
    {
        [SerializeField] private int rayLength = 5;
        [SerializeField] private LayerMask layerMaskInteract;
        [SerializeField] private string excludeLayerName = null;

        private MyDoorController raycastedDoor;
        private MyDrawerController raycastedDrawer;
        private KeyItemController raycastedKey;

        //private MyNewController raycastedObj;

        [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;

        [SerializeField] private Image crosshair = null;
        private bool isCrosshairActive;
        private bool doOnce;

        private const string DoorTag = "Door";

        private const string DrawerTag = "Drawer";

        private const string KeyTag = "Key";

        private void Update()
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, fwd, Color.green);

            int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
            {
                ///Door Interaction///

                if (hit.collider.CompareTag(DoorTag))
                {
                    if (!doOnce)
                    {
                        raycastedDoor = hit.collider.gameObject.GetComponent<MyDoorController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickDoor");
                        raycastedDoor.PlayAnimation();
                    }
                }

                ///Drawer Interaction///

                if (hit.collider.CompareTag(DrawerTag))
                {
                    if (!doOnce)
                    {
                        raycastedDrawer = hit.collider.gameObject.GetComponent<MyDrawerController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickDrawer");
                        raycastedDrawer.PlayAnimation();
                    }
                }

                ///Key Interaction///

                if (hit.collider.CompareTag(KeyTag))
                {
                    //Debug.Log("Yikes");
                    if (!doOnce)
                    {
                        raycastedKey = hit.collider.gameObject.GetComponent<KeyItemController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    doOnce = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        //Debug.Log("ClickKey");
                        raycastedKey.ObjectInteraction();
                    }
                }

            }

            else
            {
                CrosshairChange(false);
                doOnce = false;

                //if (isCrosshairActive)
                //{
                //    CrosshairChange(false);
                //    doOnce = false;
                //}
            }
        }

        void CrosshairChange(bool on)
        {
            if (on && !doOnce)
            {
                crosshair.color = Color.red;
            }

            else
            {
                crosshair.color = Color.white;
                isCrosshairActive = false;
            }
        }
    }
}
