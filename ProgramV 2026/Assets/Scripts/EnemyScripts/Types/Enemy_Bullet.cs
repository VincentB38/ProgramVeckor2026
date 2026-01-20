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

    // LayerMask for enemies (optional, ensures raycast only hits enemies)
    [SerializeField] private LayerMask playerLayer;

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
        Vector2 currentPos = rb.position;
        Vector2 nextPos = currentPos + moveDirection * speed * Time.fixedDeltaTime;

        // Raycast to prevent tunneling through enemies
        RaycastHit2D hit = Physics2D.Linecast(currentPos, nextPos, playerLayer);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<PlayerHandler>()?.ChangeHealth(-damage);
            Destroy(gameObject);
            return;
        }

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
