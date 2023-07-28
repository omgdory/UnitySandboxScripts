using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabItem : MonoBehaviour
{

    private bool inRange = false;
    private GameObject player;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private string itemName;


    // Trigger if within range of player (can be picked up)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            inRange = true;
        }
    }

    // Not in range to pick up the item
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            inRange = false;
        }
    }

    // Press F to pick up
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(inRange && player != null)
            {
                grabbed();
            }
        }
    }

    // Delete object as it is picked up
    private void grabbed()
    {
        Debug.Log("player grabbed item");
        _inventory.GetComponent<inventoryHandler>().CollectedItem(itemName);
        Destroy(this.gameObject);
    }
}
