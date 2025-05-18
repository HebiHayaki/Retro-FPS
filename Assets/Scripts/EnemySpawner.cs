using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; 

    [SerializeField]
    private float minSpawnTime = 1f; 
    [SerializeField]
    private float maxSpawnTime = 3f; 

    [SerializeField]
    private int maxEnemies = 20; 

    private Vector2 spawnAreaMin; 
    private Vector2 spawnAreaMax; 

    private float timeUntilSpawn;
    private int currentEnemyCount = 0;

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
            Debug.LogError("EnemySpawner: BoxCollider2D not found to determine spawn area!");
        }

        SetTimeUntilSpawn();
    }

    void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            timeUntilSpawn -= Time.deltaTime;

            if (timeUntilSpawn <= 0)
            {
                SpawnEnemy();
                SetTimeUntilSpawn();
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyController enemyScript = enemy.GetComponent<EnemyController>();
        if (enemyScript != null)
        {
            enemyScript.Initialize(this);
        }

        currentEnemyCount++;
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    public void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }
}
