using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    [Header("Folder / Parent")]
    public Transform enemyFolder; // Drag an empty GameObject here

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Put the enemy into the folder
        if (enemyFolder != null)
        {
            enemy.transform.SetParent(enemyFolder);
        }
    }
}
