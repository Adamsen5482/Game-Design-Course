using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GameManager Ínstance
    public static GameManager gameManager
    {

        get
        {
            return instance;
        }

        private set
        {
            if (instance != null)
            {
                Destroy(value.gameObject);
                return;
            }

            instance = value;

        }
    }
    private static GameManager instance = null; // Singleton Instance
    #endregion

    [SerializeField] private Transform[] spawnPoints = null;

    private void Awake()
    {
        gameManager = this;
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].GetComponent<ItemController>().itemID = i;
        }
    }

}
