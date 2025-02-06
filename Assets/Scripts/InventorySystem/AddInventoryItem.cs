using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddInventoryItem : MonoBehaviour
{
    protected GameObject ItemGameObject;
    private CharacterController2D PlayerController;
    public InventoryItem inventoryItem;
    [SerializeField] private Transform Player;
    private float SliderTime = 3f;
    private float elapsedTime = 0f;
    [SerializeField] private Slider timeSlider;
    private bool clicked = false;

    private void Start()
    {
        PlayerController = Player.GetComponent<CharacterController2D>();
    }



    public virtual void Update()
    {
        if (clicked)
        {
            timeSlider.gameObject.SetActive(true);
            ActivateTimeBar();
        }
        
    }

    public void ItemClicked()  // If the player is close enough to the object, then it is considered clicked
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(Player.position.x, Player.position.y)) < 3f)
        {
            clicked = true;
        }
    }

    private void ActivateTimeBar()
    {
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / SliderTime;
        timeSlider.value = Mathf.Lerp(2, 0, t);

        if (timeSlider.value <= 0)
        {
            timeSlider.value = 0;
            enabled = false;
            timeSlider.gameObject.SetActive(false);
            clicked = false;
            Inventory.instance.AddItemToInventory(inventoryItem);
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()  // Function to manage mouse click
    {
        ItemClicked();
        PlayerController.Attack();
    }

}
