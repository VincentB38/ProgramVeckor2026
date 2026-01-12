using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [HideInInspector] public EnemySpawnManager spawner;

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed();
        }
    }
}
