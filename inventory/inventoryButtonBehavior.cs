using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryButtonBehavior : MonoBehaviour
{
    public inventoryHandler inventoryReference;

    private void Start()
    {
        inventoryReference = GetComponent<inventoryHandler>();
    }

    public void OnInventorySlotClicked()
    {
        inventoryHandler.Item clickedItem = new inventoryHandler.Item("testItem");
        Debug.Log(clickedItem.itemName);
    }
}
