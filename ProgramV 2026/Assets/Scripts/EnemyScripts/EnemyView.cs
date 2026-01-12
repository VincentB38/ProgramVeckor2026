using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    Enemy enemy;

    public void InIt(Enemy enemy)
    {
        this.enemy = enemy;

        enemy.SetRigidBody2D(rb);
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }
}
