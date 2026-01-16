using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    public float wavePopDuration = 0.5f;   // How long it grows/shrinks
    public float waveDisplayDuration = 1f; // How long it stays on full size

    [Header("Heart Spawn Settings")]
    public GameObject heartPrefab;
    public GameObject goldenHeartPrefab;

    private GameObject Player;

    public float goldenHeartChance = 0.10f; // 0.1 = 10%, 1 = 100%
    public List<Transform> heartSpawnPoints = new List<Transform>();
    public Transform InteractFolder;
    public GameObject waveShipPrefab;

    public int minHeartsPerWave = 1;
    public int maxHeartsPerWave = 3;

    public SoundManager soundManager;

    private void Start()
    {

        waveText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        waveText.rectTransform.localScale = Vector3.zero; // start invisible

        Player = GameObject.Find("Player");

        if (soundManager == null)
        {
            soundManager = GetComponent<SoundManager>();
        }

        if (waves.Count > 0)
        {
            StartCoroutine(RunWave(waves[0]));
        }
        else
        {
            Debug.LogWarning("No waves configured!");
        }
    }

    private IEnumerator ShowWaveText(string text)
    {
        if (waveText == null)
            yield break;

        // Plays wave sound to alert player
        soundManager.PlaySound(1, Player.transform);

        waveText.gameObject.SetActive(true);
        waveText.text = text;

        Vector3 startScale = Vector3.zero;
        Vector3 fullScale = Vector3.one;
        waveText.transform.localScale = startScale;

        // Grow
        float t = 0f;
        while (t < wavePopDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(0f, 1f, t / wavePopDuration);
            waveText.transform.localScale = fullScale * scale;
            yield return null;
        }
        waveText.transform.localScale = fullScale;

        // Hold full size
        yield return new WaitForSeconds(waveDisplayDuration);

        // Shrink
        t = 0f;
        while (t < wavePopDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(1f, 0f, t / wavePopDuration);
            waveText.transform.localScale = fullScale * scale;
            yield return null;
        }

        waveText.gameObject.SetActive(false);
    }


    private IEnumerator RunWave(EnemyWave wave)
    {
        yield return StartCoroutine(ShowWaveText($"Wave {currentWaveIndex + 1}: {wave.waveName}"));

        // 1s delay before enemies spawn
        yield return new WaitForSeconds(1f);

        Debug.Log($"Starting Wave: {wave.waveName}");

        foreach (var group in wave.groups) // Loop through the groups in the waves
        {
            int spawnedInGroup = 0;
            while (spawnedInGroup < group.quantity)
            {
                if (aliveEnemies < group.maxAliveEnemies) // if enemies are less than max then spawn
                {
                    SpawnEnemy(group);
                    spawnedInGroup++;
                }
                yield return new WaitForSeconds(group.spawnInterval);
            }
        }

        // Wait until all enemies die
        while (aliveEnemies > 0)
            yield return null;

        if (waveShipPrefab != null)
        {
            GameObject ship = Instantiate(waveShipPrefab);
        }
        SpawnHeartsAfterWave(); // Spawn hearts after wave

        currentWaveIndex++;

        if (currentWaveIndex < waves.Count)
            yield return StartCoroutine(RunWave(waves[currentWaveIndex]));
        else // If u beat the game
            yield return StartCoroutine(ShowWaveText("All waves completed!"));

        yield return new WaitForSeconds(2f);

        Player.GetComponent<PlayerHandler>().EndGame("True"); // win
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
