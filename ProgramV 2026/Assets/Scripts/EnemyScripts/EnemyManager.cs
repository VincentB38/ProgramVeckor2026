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
        tempEnemy.SetValues(2f, 1.5f);

        et = Instantiate(enemyPrefab, new Vector3(), Quaternion.identity);
        et.InIt(tempEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        et.GetEnemy().Move(CheckPlayerPosition(et), playerTransform, et.transform);
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
