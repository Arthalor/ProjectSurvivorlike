using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Inventory : MonoBehaviour
{
    public List<Item> possibleItems = new List<Item>();

    public List<Item> items = new List<Item>();

    public void PickUpItem(Item item) 
    {
        items.Add(item);
    }

    public void PickUpItem() 
    {
        int randomIndex = Random.Range(0, possibleItems.Count);
        items.Add(possibleItems[randomIndex]);
    }
}