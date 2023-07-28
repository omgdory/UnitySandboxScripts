using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class inventoryHandler : MonoBehaviour
{


    [System.Serializable]
    public class Item
    {
        public string itemName = "";
        public GameObject imageInInventory;
        public Item(string itemName)
        {
            this.itemName = itemName;
        }



        // Not using constructor since inventory is hardcoded to 10
        public void MakeAnItemPlease(string itemName, Sprite assignedSprite)
        {
            this.itemName = itemName;
            this.imageInInventory.SetActive(true);
            this.imageInInventory.GetComponent<Image>().sprite = assignedSprite;
        }

    }

    [SerializeField] private Sprite[] myGlobalSprites; // Sprites

    // Better to dynamically allocate if using a constructor to put item into inventory
    public Item[] inventoryItems = new Item[10];

    private bool inventoryOpen = false;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject myGameObjects;
    GameObject[] disabledItems = new GameObject[100];
    public void onBackButton()
    {
        // Turn off inventory
        inventoryOpen = false;
        inventory.SetActive(false);
        disableStuff(false);
    }

    public void onInventorySlotSelected(GameObject itself)
    {
        // Better way; instantiate slots as UI elements and add to hash map as a key
        // Pass item name as value
        #region ButtonIDCheck
        if(itself == null)
        {
            Debug.Log("Button Not Instantiated");
            return;
        }
        #endregion

    }

    // Ensure that interactables and player are off when inventory is open
    private void disableStuff(bool turnOff)
    {
        Transform[] myChildren = myGameObjects.transform.GetComponentsInChildren<Transform>();

        if(turnOff)
        {
            int i = 0;
            foreach(Transform thing in myChildren)
            {
                if(i == 0) {
                    i++;
                } else {
                    disabledItems[i] = thing.gameObject;
                    thing.gameObject.SetActive(false);
                    i++;
                }
            }
            return;
        }

        for(int i = 0; i < disabledItems.Length; i++)
        {
            if(disabledItems[i] != null) {
                disabledItems[i].SetActive(true);
                disabledItems[i] = null;
            }
        }
    }

    // Put in inventory
    public void CollectedItem(string pickedUpItemName)
    {
        int spriteID = 0;
        if(pickedUpItemName == "Barack Buck")
        {
            spriteID = 1;
        }
        // Go through inventory slots to find an empty slot (null or whitespace)
        int index = 0;
        foreach(Item item in inventoryItems)
        {
            if((item.itemName == null) || string.IsNullOrEmpty(item.itemName) || string.IsNullOrWhiteSpace(item.itemName))
            {
                // Will enable image in inventory
                Debug.Log("found open inventory slot");
                inventoryItems[index].MakeAnItemPlease(pickedUpItemName, myGlobalSprites[spriteID]);

                // Exit if found an open slot
                return;
            }

            index++;
        }
    }

    private void Start()
    {
        inventoryOpen = false;
        inventory.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(!inventoryOpen) {
                inventoryOpen = true;
                inventory.SetActive(true);
                disableStuff(true);
            } else {
                inventoryOpen = false;
                inventory.SetActive(false);
                disableStuff(false);
            }
        }
    }
}
