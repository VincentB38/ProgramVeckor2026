using UnityEngine;

public class KillFloorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHandler>().ChangeHealth(-50);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Collider2D>().enabled = false;
            Destroy(collision.gameObject);
        }
    }
}
