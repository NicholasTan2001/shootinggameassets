using UnityEngine;

public class SpiderRandomSpawn : MonoBehaviour
{
    public GameObject spiderPrefab; 
    public int spawnCount = 5; 
    public float spawnWidth = 10f; 
    public float spawnLength = 10f; 
    public float spawnInterval = 30f; 

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 center = transform.position;
        Vector3 halfSize = new Vector3(spawnWidth / 2f, 0f, spawnLength / 2f);
        Gizmos.DrawWireCube(center, new Vector3(spawnWidth, 0.1f, spawnLength));
    }

    public void SpawnSpiders()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-spawnWidth / 2f, spawnWidth / 2f), 0f, Random.Range(-spawnLength / 2f, spawnLength / 2f));

            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);

            Instantiate(spiderPrefab, randomPosition, rotation);
        }
    }

    void Start()
    {
        InvokeRepeating("SpawnSpiders", spawnInterval, spawnInterval);
    }
}

