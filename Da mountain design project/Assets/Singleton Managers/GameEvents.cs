using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region GameEvents Instance 
    public static GameEvents gameEvents
    {
        get
        {
            return instance;
        }

        set
        {
            if (instance != null && instance != value)
            {
                Destroy(value.gameObject);
                return;
            }

            instance = value;
        }
    }
    private static GameEvents instance = null; // Singleton Instance
    #endregion

    public event Action<int> onItemPickUpEnter; // Item pick up event

    private void Awake()
    {
        gameEvents = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ItemPickUpTriggerEnter(int id)
    {
        onItemPickUpEnter?.Invoke(id); // Invoke event if not equal to null. Only invokes for items with id argument.
    }
}
