using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseButton : MonoBehaviour, IPointerClickHandler
{
    private DetailsManager detailsManager;
    private GameObject infoPanel;
    private int ChildNumber;
    private string ItemName;
    private InventoryItem inventoryItem;
    private CharacterController2D Player;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            findItemName(); 
            inventoryItem = Inventory.instance.GetItemByName(ItemName);
            Inventory.instance.RemoveQuantityFromInventory(inventoryItem);
            Player.setItem(inventoryItem); 
            detailsManager.DeactivateInventory();
            detailsManager.ActivateInventoryPanel();
            detailsManager.ActivateUI();
            Destroy(transform.parent.gameObject);
        }
    }

    void Start()
    {
        GameObject InventoryGameObject = GameObject.Find("InventoryManager");
        detailsManager = InventoryGameObject.GetComponent<DetailsManager>();
        infoPanel = detailsManager.getItemInfoGameObject();
        Player = GameObject.Find("Player").GetComponent<CharacterController2D>();
    }

    private void findItemName()   // Finds the name of the item in the inventoryslot pressed
    {
        for (int i = 0; i < transform.parent.parent.parent.childCount; i++)
        {
            if (transform.parent.parent.parent.GetChild(i) == transform.parent.parent) // grid child
            {
                ChildNumber = i;
            }
        }
        ItemName = Inventory.instance.GetItemByIndex(ChildNumber).name;
    }


}
