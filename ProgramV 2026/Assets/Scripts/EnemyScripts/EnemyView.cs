using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    Enemy enemy;

    // Variables for layer movement
    bool movedToLayer;
    float timeUntilMoveEnables;

    public void InIt(Enemy enemy)
    {
        this.enemy = enemy;

        enemy.SetRigidBody2D(rb);
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            enemy.LayerMove(collision.gameObject, enemy.gameObject);
        }
    }
}
