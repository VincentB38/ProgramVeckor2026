using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    private int damage;
    private float speed;
    private int pointLoss;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody settings for fast bullets
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        SetDirectionTowardsPlayer();

        // Ensure collider is a trigger
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection.normalized * speed;
    }

    public void SetValues(Vector2 direction, float bulletSpeed, int dmg, int pointLoss)
    {
        moveDirection = direction;
        speed = bulletSpeed;
        damage = dmg;
        this.pointLoss = pointLoss;
    }

    private void SetDirectionTowardsPlayer()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHandler>().ChangeHealth(-damage);
            other.GetComponent<PlayerHandler>().UpdateScore(-pointLoss);
            Destroy(gameObject);
        }
    }
}
