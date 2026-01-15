using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    private int damage;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

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
        SetDirectionTowardsMouse();

        // Ensure collider is a trigger
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = rb.position;
        Vector2 nextPos = currentPos + moveDirection * speed * Time.fixedDeltaTime;

        // Raycast to prevent tunneling through enemies
        RaycastHit2D hit = Physics2D.Linecast(currentPos, nextPos, enemyLayer);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        rb.MovePosition(nextPos);
    }

    private void SetDirectionTowardsMouse()
    {
        if (Mouse.current == null) return;

        Vector3 mousePosScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        moveDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
