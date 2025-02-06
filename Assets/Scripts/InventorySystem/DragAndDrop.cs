using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    InventoryItem inventoryItem;
    CanvasGroup group;
    public Transform parentAfterDrag;

    private Transform originalParent;
    private int originalSiblingIndex;
    private CanvasGroup canvasGroup;
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Saves the child index of the parent to be able to change the order of the dictionary in Inventory
        originalSiblingIndex = transform.GetSiblingIndex();

        // Saves the original parent
        originalParent = transform.parent;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false; // Deactivates the raycasts while dragging
        }

        // Sets the item above the other items in the hierarchy to be able to see it
        transform.SetParent(originalParent.parent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Sets the parent to the original one
        transform.SetParent(originalParent);

        // Finds the object under the pointer 
        int newSiblingIndex = -1;

        foreach (Transform child in originalParent)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(child as RectTransform, eventData.position))
            {
                newSiblingIndex = child.GetSiblingIndex();  // (Child) Index of the item under the pointer
                break;
            }
        }

        if (newSiblingIndex != -1) // If the pointer is above another item
        {
            transform.SetSiblingIndex(newSiblingIndex); 
            Inventory.instance.changeDictionaryOrder(originalSiblingIndex, newSiblingIndex);
        }
        else 
        {
            transform.SetSiblingIndex(originalSiblingIndex); 
        }

        // Reactivate the raycasts after dropping the item
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}




