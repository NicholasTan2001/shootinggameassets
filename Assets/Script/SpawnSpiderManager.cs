using UnityEngine;
using System;

public class SpiderSpawnManager : MonoBehaviour
{
    public GameObject spiderPrefab; 
    public Transform spawnPoint; 
    public float respawnDelay = 8f;

    private bool spawning = false; 

    void Start()
    {
        SpawnSpider();
    }

    void SpawnSpider()
    {
        if (spiderPrefab != null && spawnPoint != null && !spawning)
        {
            spawning = true; 
            GameObject spawnedSpider = Instantiate(spiderPrefab, spawnPoint.position, spawnPoint.rotation);
           
            spawnedSpider.GetComponent<Spider>().OnSpiderDestroyed += RespawnSpider;
        }
        else if (spawning)
        {
            Debug.LogWarning("A spider is already spawning. Delaying next spawn.");
        }
        else
        {
            Debug.LogError("Spider prefab or spawn point is not assigned in the SpiderSpawnManager.");
        }
    }

    void RespawnSpider()
    {
        Invoke("SpawnSpider", respawnDelay);
        spawning = false; 
    }
}
