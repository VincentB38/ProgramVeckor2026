using Unity.VisualScripting;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bouncePower;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = -collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX * bouncePower;
        }
    }
}
