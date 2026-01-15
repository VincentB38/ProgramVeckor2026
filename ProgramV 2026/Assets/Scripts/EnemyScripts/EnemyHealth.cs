using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;
    [SerializeField] private int ScoreGain = 30;
    private GameObject Player;

    private void Awake()
    {
        currentHealth = maxHealth;
        Player = GameObject.Find("Player");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();

            Player.GetComponent<PlayerHandler>().UpdateScore(ScoreGain);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
