using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [HideInInspector] public int itemID;
    public GameObject item;
    //Player player;
    private GameObject itemGameObject;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.gameEvents.onItemPickUpEnter += onItemPickUp;
        SpawnItem(item);
        itemGameObject = gameObject.transform.GetChild(0).gameObject;
    }

    private void onItemPickUp(int id)
    {
        if (id == itemID)
        {
            print("Enter!");
            AudioManager.audioManager.PlaySFX(AudioManager.audioManager.sfxObjects[0].audioClip);
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
        Item itemObject = item.GetComponent<Item>();
        itemObject.itemID = itemID;
    }

    private void OnDestroy()
    {
        GameEvents.gameEvents.onItemPickUpEnter -= onItemPickUp;
    }

}
