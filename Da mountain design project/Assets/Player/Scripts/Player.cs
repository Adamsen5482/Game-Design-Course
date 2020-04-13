using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
    }

    // Only for testiing
    public void PrintItem()
    {
        print(items[0]);
    }

    public bool CanHook()
    {
        bool hook = false, rope = false;
        foreach (GameObject item in items)
        {
            hook = item.GetComponent<Item>().itemType == Item.ItemType.Hook ? true : false;
            rope = item.GetComponent<Item>().itemType == Item.ItemType.Rope ? true : false;
        }

        return (hook && rope) ? true : false; 
    }
    
}
