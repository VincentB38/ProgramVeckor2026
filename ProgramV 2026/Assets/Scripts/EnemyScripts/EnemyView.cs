using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    Enemy enemy;

    public void InIt(Enemy enemy)
    {
        this.enemy = enemy;

        rb = enemy.GetRigidBody2D();
    }
}
