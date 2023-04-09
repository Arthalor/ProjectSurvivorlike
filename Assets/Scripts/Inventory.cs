using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void PickUpItem(Item item) 
    {
        items.Add(item);
    }
}