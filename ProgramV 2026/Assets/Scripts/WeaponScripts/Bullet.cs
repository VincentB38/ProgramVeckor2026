using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
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

        if (collision.gameObject.CompareTag("OnGround"))
        {
            Destroy(gameObject);
        }
    }
}
