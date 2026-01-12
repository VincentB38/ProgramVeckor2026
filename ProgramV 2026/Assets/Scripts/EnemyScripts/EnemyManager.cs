using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyView enemyPrefab;
    [SerializeField] Transform playerTransform;
    

    public float tempSpeed;

    EnemyView et;
    void Start()
    {
        // Unity decided to be annoying - Has to add, removes the point of constructor
        Enemy tempEnemy = gameObject.AddComponent<Enemy_Melee>();
        tempEnemy.SetValues(2f, 3);

        et = Instantiate(enemyPrefab, new Vector3(), Quaternion.identity);
        et.InIt(tempEnemy);

        if (tempEnemy == null)
        {
            Debug.LogError("tempEnemy is null!");
        }
        else if (et.GetEnemy() == null)
        {
            Debug.LogError("GetEnemy() returned null!");
        }
        if (playerTransform == null)
        {
            Debug.LogError("playerTransform is null!");
        }
        else
        {
            et.GetEnemy().Move(CheckPlayerPosition(et), playerTransform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        et.GetEnemy().Move(CheckPlayerPosition(et), playerTransform);
    }

    // True if the player is to the right and false if left
    int CheckPlayerPosition(EnemyView enemy)
    {
        if (playerTransform.position.x < enemy.transform.position.x)
        {
            return -1; // Player is on the left
        }
        else
        {
            return 1; // Player is to the right
        }
    }
}
