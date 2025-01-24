using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    public GameObject shopObject;
    public bool shopHasSpawned;
    private float timer = 0;
    public int seconds = 0;
    public int firstSpawnInterval;
    public int secondSpawnInterval;
    
    void Update()
    {
        // seconds in float
        timer += Time.deltaTime;

        // turn seconds in float to int
        seconds = (int)(timer % 60);

        if (!shopHasSpawned)
        {
            if(seconds >= firstSpawnInterval)
            {
                print("Spawn first shop");
                shopObject.SetActive(true);
                shopHasSpawned = true;
                timer = 0;
            }
        }
        else
        {
            if (seconds >= secondSpawnInterval)
            {
                print("Spawn second shop");
                shopObject.SetActive(true);
                timer = 0;
            }
        }
    }
}
