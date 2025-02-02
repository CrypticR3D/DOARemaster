using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPlacing : MonoBehaviour
{
    [SerializeField] ItemData[] placeableItem;

    //IMPORTANT FOR DESIGNERS IF UR SNOOPING: read the below tooltip before changing anything. It may lead to you being able to fix the issue you have

    [Tooltip("This should be the local position of where the placeable item should be placed. If you also need the rotation and scale changed, change that in the prefab of the object itself")]
    [SerializeField] Vector3 placeableItemPosition;

    Inventory inventory;

    internal bool isItemPlaced = false;

    GameObject placedItem;
    public string clipName;//Matej changes


    public enum Tags
    {
        redTag,
        blueTag,
        yellTag,
        greenTag
    }

    public new Tags tag;
    public bool correctHead;


    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (!Input.GetButtonDown("Interact")) return; //hate having such a long if chain so i did this. Dont care. If ur changing this script look at this line.
        if (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))
        {
            if(hit.transform.gameObject == placedItem)
            {
                isItemPlaced = false;
                correctHead = false;
                placedItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                placedItem = null;
            }
            if (hit.transform.gameObject == gameObject)
            {
                for(int i = 0; i < placeableItem.Length; i++)
                {
                    if (inventory.itemInventory[inventory.selectedItem] == placeableItem[i]) //if the selected item is the one needed
                    {
                        //place item
                        placedItem = Instantiate(placeableItem[i].prefab);
                        SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);//Matej changes

                        switch (tag)
                        {
                            case Tags.redTag:
                                if (placedItem.CompareTag("redHead"))
                                {
                                    correctHead = true;
                                }
                                break;
                            case Tags.blueTag:
                                if (placedItem.CompareTag("blueHead"))
                                {
                                    correctHead = true;
                                }
                                break;
                            case Tags.yellTag:
                                if (placedItem.CompareTag("yellHead"))
                                {
                                    correctHead = true;
                                }
                                break;
                            case Tags.greenTag:
                                if (placedItem.CompareTag("greenHead"))
                                {
                                    correctHead = true;
                                }
                                break;
                        }

                        //Destroy(placedItem.GetComponent<Rigidbody>());
                        //Destroy(placedItem.GetComponent<GlowWhenLookedAt>());
                        //Destroy(placedItem.GetComponent<HoldableItem>());

                        //Make Placed item frozen
                        placedItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


                        //set this gameobject to be the placed items parent
                        placedItem.transform.parent = transform;
                        placedItem.transform.localPosition = placeableItemPosition;
                        isItemPlaced = true;
                        //remove item from inventory
                        inventory.removeItem();
                    }
                }
                
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(placeableItemPosition), 0.1f);
    }
}