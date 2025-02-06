using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : MonoBehaviour
{
    [SerializeField] private GameObject itemInfo;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject UI;

    public void activateItemInfo()  // Activates the ItemInfo panel
    {
        itemInfo.SetActive(true);
    }

    public GameObject getItemInfoGameObject() // Return the ItemInfo panel
    {
        return itemInfo;
    }

    public void DeactivateInventoryPanel() // Deactivates the InventoryPanel
    {
        InventoryPanel.SetActive(false);
    }

    public void ActivateInventoryPanel() // Activates the InventoryPanel
    {
        InventoryPanel.SetActive(true);
    }

    public void ActivateInventory() // Activates the Inventory
    {
        Inventory.SetActive(true);
    }

    public void DeactivateInventory() // Activates the Inventory
    {
        Inventory.SetActive(false);
    }

    public void ActivateUI() // Activates the UI panel
    {
        UI.SetActive(true);
    }

}
