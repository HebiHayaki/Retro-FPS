using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemPrefabs; 

    [SerializeField]
    private float minSpawnTime = 5f; 
    [SerializeField]
    private float maxSpawnTime = 10f; 

    [SerializeField]
    private int maxItems = 10; 

    [SerializeField]
    private Vector2 spawnAreaMin; 
    [SerializeField]
    private Vector2 spawnAreaMax; 

    private float timeUntilSpawn;
    private int currentItemCount = 0;

    void Awake()
    {
        BoxCollider2D spawnArea = GetComponent<BoxCollider2D>();
        if (spawnArea != null)
        {
            spawnAreaMin = spawnArea.bounds.min;
            spawnAreaMax = spawnArea.bounds.max;
        }
        else
        {
            Debug.LogError("ItemSpawner: BoxCollider2D not found to determine spawn area!");
        }

        SetTimeUntilSpawn();
    }

    void Update()
    {
        if (currentItemCount < maxItems)
        {
            timeUntilSpawn -= Time.deltaTime;

            if (timeUntilSpawn <= 0)
            {
                SpawnItem();
                SetTimeUntilSpawn();
            }
        }
    }

    private void SpawnItem()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

        var pickup = item.GetComponent<MonoBehaviour>();
        if (pickup is AmmoPickup ammoPickup)
        {
            ammoPickup.Initialize(this);
        }
        else if (pickup is HealthPickup healthPickup)
        {
            healthPickup.Initialize(this);
        }

        currentItemCount++;
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    public void DecreaseItemCount()
    {
        currentItemCount--;
    }
}
