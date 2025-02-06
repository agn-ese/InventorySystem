using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Dictionary<InventoryItem, int> inventoryItems = new Dictionary<InventoryItem, int>();

    [SerializeField] private Transform ItemContent;
    [SerializeField] private GameObject InventoryItem;

    // Singleton

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

 

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks if the item is already in the inventory
    // If not, then it adds it
    // If it's already in there, adds the quantity relative to the InventoryItem
    public void AddItemToInventory(InventoryItem Item) 
    {
        if(inventoryItems.ContainsKey(Item))
            inventoryItems[Item] += Item.quantity;
        else 
            inventoryItems.Add(Item, Item.quantity);
    }

    // Removes the quantity of the InventoryItem from the inventory
    // If the quantity becomes 0, then it removes the item from the inventory
    public void RemoveQuantityFromInventory(InventoryItem Item)
    {
        if (inventoryItems.ContainsKey(Item) && inventoryItems[Item] != 0)
        {
            inventoryItems[Item] -= Item.quantity;
        }
        if (inventoryItems[Item] == 0)
            inventoryItems.Remove(Item);
    }

    // Returns the quantity of an item

    public int GetQuantity(InventoryItem Item)
    {
        return inventoryItems[Item];
    }

    // Function used for listing the items in the inventory when pressing the inventory button
    public void ListItems()
    {
        foreach(Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach(InventoryItem Item in inventoryItems.Keys)
        {
            GameObject InstantiatedSlot = Instantiate(InventoryItem, ItemContent);
            var ItemQuantity = InstantiatedSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            var ItemIcon = InstantiatedSlot.transform.GetChild(0).GetComponent<Image>();

            ItemQuantity.text = Item.quantity.ToString();
            ItemIcon.sprite = Item.itemImage;
        }
    }

    // Returns the inventoryItem knowing its index in the dictionary
    public InventoryItem GetItemByIndex(int index)
    {
        InventoryItem keyAtIndex = null;
        if (index >= 0 && index < inventoryItems.Count)
        {
            int currentIndex = 0;

            foreach (InventoryItem key in inventoryItems.Keys)
            {
                if (currentIndex == index)
                {
                    keyAtIndex = key;
                    break;
                }
                currentIndex++;
            }

        }
        else
        {
            Debug.Log("Index out of range");
        }
        return keyAtIndex;
    }

    // Updates the order of the dictionary based on the drag and drop functionality
    public void changeDictionaryOrder(int indexBefore, int indexAfter)
    {
        // Converts the dictionary into a List of tuples
        List<KeyValuePair<InventoryItem, int>> itemsList = new List<KeyValuePair<InventoryItem, int>>(inventoryItems);
        if (indexBefore >= 0 && indexBefore < itemsList.Count && indexAfter >= 0 && indexAfter < itemsList.Count)
        {
            // Swaps the elements
            var temp = itemsList[indexBefore];
            itemsList[indexBefore] = itemsList[indexAfter];
            itemsList[indexAfter] = temp;

            inventoryItems.Clear(); // Empties the already existing dictionary
            foreach (var item in itemsList)
            {
                inventoryItems.Add(item.Key, item.Value); // Reinserts the items in the dictionary with the correct order
            }
        }
        else
        {
            Debug.LogError("Indici non validi per il dizionario.");
        }
    }


    // Returns an InventoryItem knowing its name
    public InventoryItem GetItemByName(string name)
    {
        foreach (InventoryItem item in inventoryItems.Keys)
        {
            if (item.name == name)
                return item;
        }
        return null;
    }


}
