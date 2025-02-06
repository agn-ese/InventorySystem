using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class InventoryItem : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemImage;
    public int quantity;
    public GameObject prefab;            // Prefabs to instantiate (for rock and wood)
    public float throwForce = 10f;       // Used for throwing rock
    public float angle = 45f;            // Used for parabolic throw
    public float slowDuration = 7f;      // Duration of slow effect

}

