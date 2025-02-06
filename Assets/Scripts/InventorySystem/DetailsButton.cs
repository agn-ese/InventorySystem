using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsButton : MonoBehaviour, IPointerClickHandler
{
    private DetailsManager detailsManager;
    private GameObject infoPanel;
    private int ChildNumber;
    public bool buttonPressed = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buttonPressed = true;
            detailsManager.activateItemInfo();
            GetInfo();
            detailsManager.DeactivateInventoryPanel();
            Destroy(transform.parent.gameObject);
        }
    }

    void Start()
    {
        GameObject InventoryGameObject = GameObject.Find("InventoryManager");
        detailsManager = InventoryGameObject.GetComponent<DetailsManager>();
    }


    public void GetInfo() // Finds the info about the item in the inventory slot pressed
    {
        detailsManager.activateItemInfo();
        infoPanel = detailsManager.getItemInfoGameObject();
        infoPanel.transform.Find("QuantityNum").GetComponent<TextMeshProUGUI>().text = transform.parent.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        for (int i = 0; i < transform.parent.parent.parent.childCount; i++)
        {
            if (transform.parent.parent.parent.GetChild(i) == transform.parent) // grid child
            {
                ChildNumber = i;
            }
        }
        infoPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = Inventory.instance.GetItemByIndex(ChildNumber).name;
        infoPanel.transform.Find("Icon").GetComponent<Image>().sprite = transform.parent.parent.GetChild(0).GetComponent<Image>().sprite;
    }


}
