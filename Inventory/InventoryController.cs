using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    void Start()
    {
        inventory = transform.Find("Inventory").GetComponent<Inventory>();
        //inventory = GetComponentInChildren<Inventory>();
        inventory.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventory.gameObject.activeSelf)
            {
                inventory.gameObject.SetActive(false);
            }
            else
            {
                inventory.gameObject.SetActive(true);
            }
        }
    }
}
