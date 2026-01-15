using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    private int damage;

    private Vector2 moveDirection;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        SetDirectionTowardsMouse();
    }

    private void Update()
    {
        // Move the bullet
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void SetDirectionTowardsMouse()
    {
        if (Mouse.current == null) return; // Safety check

        // Get mouse position in screen space
        Vector3 mousePosScreen = Mouse.current.position.ReadValue();

        // Convert to world position
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mouseWorldPos.z = 0f; // 2D plane

        // Calculate direction
        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        moveDirection = direction;

        // Rotate bullet to face mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject
                .GetComponent<EnemyHealth>()
                ?.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
