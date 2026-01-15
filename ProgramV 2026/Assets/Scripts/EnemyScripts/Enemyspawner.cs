using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    [Header("Limits")]
    public int maxTotalSpawns = 20;
    public int maxAliveEnemies = 5;

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Folder / Parent")]
    public Transform enemyFolder;

    private int totalSpawned = 0;
    private int aliveEnemies = 0;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (totalSpawned >= maxTotalSpawns)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        if (aliveEnemies >= maxAliveEnemies)
            return;

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        if (enemyFolder != null)
            enemy.transform.SetParent(enemyFolder);

        totalSpawned++;
        aliveEnemies++;

        EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
        tracker.spawner = this;
    }

    public void OnEnemyDestroyed()
    {
        aliveEnemies--;
    }
}
