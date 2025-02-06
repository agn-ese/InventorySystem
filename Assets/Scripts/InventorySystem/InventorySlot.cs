using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    private DetailsManager DetailsManager;

    [SerializeField] private GameObject ChoicePanel; //Panel for choosing between viewing item info or using item


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject Choice = Instantiate(ChoicePanel, transform);

        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }

        
    }

}
