using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Add this for TextMeshPro

[System.Serializable]
public class EnemyGroup // Creates the information for each group
{
    public GameObject enemyPrefab;
    public int quantity = 1;
    public float spawnInterval = 0.5f;
    public List<Transform> spawnPoints = new List<Transform>();
    public int maxAliveEnemies = 3;
}

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<EnemyGroup> groups = new List<EnemyGroup>();
}

public class EnemySpawnManager : MonoBehaviour
{
    public List<EnemyWave> waves = new List<EnemyWave>(); // List of waves

    public TMP_Text waveText;

    public Transform enemyFolder;

    private int currentWaveIndex = 0;
    private int aliveEnemies = 0;

    [Header("Heart Spawn Settings")]
    public GameObject heartPrefab;
    public GameObject goldenHeartPrefab;

    public float goldenHeartChance = 0.10f; // 0.1 = 10%, 1 = 100%
    public List<Transform> heartSpawnPoints = new List<Transform>();
    public Transform InteractFolder;

    public int minHeartsPerWave = 1;
    public int maxHeartsPerWave = 3;

    private void Start()
    {
        if (waves.Count > 0)
        {
            UpdateWaveText(); // Updates wave Text
            StartCoroutine(RunWave(waves[currentWaveIndex])); // starts the wave
        }
        else // If no waves were added
        {
            Debug.LogWarning("No waves configured!");
        }
    }

    private void UpdateWaveText() // Update wave text
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWaveIndex + 1}: {waves[currentWaveIndex].waveName}";
        }
    }

    private IEnumerator RunWave(EnemyWave wave) // Starting the wave
    {
        Debug.Log($"Starting Wave: {wave.waveName}");

        UpdateWaveText();

        foreach (var group in wave.groups) // Loopthrough the group lists that are inside of the wave
        {
            int spawnedInGroup = 0;

            while (spawnedInGroup < group.quantity)
            {
                if (aliveEnemies < group.maxAliveEnemies) // Ensure that it doesn't make more enemies than max
                {
                    SpawnEnemy(group); // Spawn enemy
                    spawnedInGroup++;
                }

                yield return new WaitForSeconds(group.spawnInterval); // Wait between each spawn
            }
        }

        // Wait until all enemies from this wave are dead
        while (aliveEnemies > 0)
            yield return null;
        SpawnHeartsAfterWave(); // Once enemies are less than 0 this will run
        currentWaveIndex++;

        if (currentWaveIndex < waves.Count) // continue if more waves exist
            StartCoroutine(RunWave(waves[currentWaveIndex]));
        else
        {
            Debug.Log("All waves completed!");
            if (waveText != null)
                waveText.text = "All waves completed!";
        }
    }

    private void SpawnHeartsAfterWave()
    {
        if (heartPrefab == null || heartSpawnPoints.Count == 0) // if there's no spawnpoints 
            return;

        int heartsToSpawn = Random.Range(minHeartsPerWave, maxHeartsPerWave + 1);

        List<Transform> availablePoints = new List<Transform>(heartSpawnPoints); // Store where the heart can spawn

        for (int i = 0; i < heartsToSpawn && availablePoints.Count > 0; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[index];
            availablePoints.RemoveAt(index);

            GameObject prefabToSpawn =
                (goldenHeartPrefab != null && Random.value <= goldenHeartChance) // Chooses which heart to spawn
                ? goldenHeartPrefab
                : heartPrefab;

            GameObject heart = Instantiate(
                prefabToSpawn,
                spawnPoint.position,
                spawnPoint.rotation
            );

            if (InteractFolder != null)
                heart.transform.SetParent(InteractFolder);
        }
    }


    private void SpawnEnemy(EnemyGroup group) // Spawn enemies
    {
        if (group.spawnPoints.Count == 0) // if there is no spawnpoitns assigned
        {
            return;
        }

        Transform spawnPoint = group.spawnPoints[Random.Range(0, group.spawnPoints.Count)]; // Randomly spawn between the assigned spawnpoints
        GameObject enemy = Instantiate(group.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        aliveEnemies++;

        EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
        tracker.spawner = this;
    }

    public void OnEnemyDestroyed() // When enemy dies
    {
        aliveEnemies--;
    }
}
