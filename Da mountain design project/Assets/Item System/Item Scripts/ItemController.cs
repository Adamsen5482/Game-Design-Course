﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemID;
    public GameObject item;
    private GameObject itemGameObject;
    public GameObject crosshairCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.gameEvents.onItemPickUpEnter += onItemPickUp;
        //SpawnItem(item);
        itemGameObject = transform.GetChild(0).gameObject;
        //print(gameObject.name + " " + itemID);

    }

    private void onItemPickUp(int id)
    {
        if (id == itemID)
        {
            AudioManager.audioManager.PlaySFXObject(AudioManager.audioManager.stateObjects[(int) AudioManager.StateNum.Confirm]);
            GameObject.FindGameObjectWithTag("Player").GetComponent<GraplingHook>().canHook = true;
            Destroy(itemGameObject);
            //crosshairCanvas.SetActive(true);
            StartCoroutine(helper.instance.GetMessage("Press 'Q' to equip Rope"));
        }
    }

    private void SpawnItem(GameObject item)
    {
        Instantiate(item, gameObject.transform);
        InitializeItem(item);
    }

    private void InitializeItem(GameObject item)
    {
        item.GetComponent<GameObject>();
        item.GetComponent<Item>().itemID = itemID;
    }

    private void OnDestroy()
    {
        GameEvents.gameEvents.onItemPickUpEnter -= onItemPickUp;
    }

}
