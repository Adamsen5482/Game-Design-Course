using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [HideInInspector] public int itemID;
    public GameObject item;
    Player player;
    private GameObject itemGameObject;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.gameEvents.onItemPickUpEnter += onItemPickUp;
        SpawnItem(item);
        itemGameObject = transform.GetChild(0).gameObject;
    }

    private void onItemPickUp(int id)
    {
        if (id == itemID)
        {
            print("Enter!");
            //AudioManager.audioManager.PlaySFXObject(AudioManager.audioManager.sfxObjects[0]);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.AddItem(itemGameObject);
            Destroy(itemGameObject);
            
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
