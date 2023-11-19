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
        AddItemToInvetory(item);
    }

    public void PickUpItem() 
    {
        int randomIndex = Random.Range(0, possibleItems.Count);
        AddItemToInvetory(possibleItems[randomIndex]);
    }

    private void AddItemToInvetory(Item item) 
    {
        items.Add(item);
        GetComponent<PlayerStats>().RecalculateStats();
    }
}