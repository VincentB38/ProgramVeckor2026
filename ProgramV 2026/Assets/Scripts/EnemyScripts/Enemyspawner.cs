using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    [Header("Limits")]
    public int maxTotalSpawns = 20;   // Total enemies that can ever spawn
    public int maxAliveEnemies = 5;   // Enemies allowed alive at once

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
        // Stop if total spawn limit reached
        if (totalSpawned >= maxTotalSpawns)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        // Stop if too many enemies alive
        if (aliveEnemies >= maxAliveEnemies)
            return;

        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        if (enemyFolder != null)
            enemy.transform.SetParent(enemyFolder);

        totalSpawned++;
        aliveEnemies++;

        // Tell the enemy who spawned it
        EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
        tracker.spawner = this;
    }

    // Called when an enemy dies
    public void OnEnemyDestroyed()
    {
        aliveEnemies--;
    }
}
