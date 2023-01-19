using UnityEngine;
using System.Collections;

public class EM_PickupObject : MonoBehaviour
{

    public Camera mainCamera;
    public LayerMask layerMask;
    public float distance = 2f;
    //public GameObject[] inventory;

    Inventory inventory;
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (hit.transform.GetComponent<HoldableItem>().data)
                {
                    inventory.addItem(hit.transform.GetComponent<HoldableItem>().data);
                }

                GameObject hitObject = hit.transform.gameObject;
                Pickup(hitObject);
            }
        }
    }

    void Pickup(GameObject obj)
    {
        obj.transform.SetParent(this.transform);
        obj.SetActive(false);

        //for (int i = 0; i < inventory.Length; i++)
        //{
        //    if (inventory[i] == null)
        //    {

        //        inventory[i] = obj;
        //        break;
        //    }
        //}
    }
}
