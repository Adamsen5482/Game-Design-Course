using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Player instance
    public static Player player
    {
        get
        {
            return instance;
        }

        set
        {
            if (instance != null)
            {
                Destroy(value.gameObject);
                return;
            }

            instance = value;
        }
    }
    private static Player instance = null; // Singleton instance
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private Item item;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public Item GetItem()
    {
        return item;
    }

    public bool CanHook()   
    {
        if (item == null)
        {
            return false;
        }
        else
        {   
            return item.itemType == Item.ItemType.Rope ? true : false;
        }
    }
    
}
